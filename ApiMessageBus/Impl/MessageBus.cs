using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Exceptions;
using RabbitMQ.Client.Events;
using System.Diagnostics;
using System.Reflection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace MessageBus;

public class MessageBus(
	IChannel _channel,
	ILogger _logger,
	IServiceProvider _services) : IMessageBus
{

	private readonly List<QueueConfig> configs = new List<QueueConfig>();
	private readonly string queueList;


	public IMessageBus ConsumeQueue<T>(string queueName, int fetchCount = 1, bool durable = false, bool exclusive = false, bool autoDelete = false)
	{
		configs.Add(new QueueConfig
		{
			QueueName = queueName,
			QueueType = typeof(T),
			FetchCount = fetchCount,
			Durable = durable,
			Exclusive = exclusive,
			AutoDelete = autoDelete
		});

		return this;
	}

	public async Task Run()
	{
		foreach (var item in configs)
		{
			if (!CanInitialize(item.QueueName))
				continue;

			await DeclareQueue(item);
			await CreateConsumer(item);
		}

		new ManualResetEvent(false).WaitOne();
	}



	private bool CanInitialize(string name)
	{
		if (string.IsNullOrEmpty(queueList))
			return true;

		return queueList.Split(' ')
			.Contains(name);
	}

	private async Task DeclareQueue(QueueConfig queue)
	{
		_logger.LogInformation($"Declaring RabbitMQ queue {queue.QueueName}");
		await _channel.QueueDeclareAsync(
			queue: queue.QueueName,
			durable: queue.Durable,
			exclusive: queue.Exclusive,
			autoDelete: queue.AutoDelete,
			arguments: null);
	}

	private async Task CreateConsumer(QueueConfig queue)
	{
		_logger.LogInformation($"Creating queue consumer for {queue.QueueName}");

		var consumer = new AsyncEventingBasicConsumer(_channel);

		consumer.ReceivedAsync += QueueReceived;

		await _channel.BasicQosAsync(0, Convert.ToUInt16(queue.FetchCount), false);
		var tag = await _channel.BasicConsumeAsync(
			queue: queue.QueueName,
			autoAck: false,
			consumer: consumer);

		_logger.LogInformation($"Queue {queue.QueueName} with tag {tag} is waiting for messages.");
	}



	private async Task QueueReceived(object sender, BasicDeliverEventArgs e)
	{
		var queueName = e.RoutingKey;
		var queueConfig = configs.Single(x => x.QueueName == queueName);

		if (queueConfig?.QueueType == null)
			throw new ArgumentNullException(nameof(queueConfig.QueueType));

		_logger.LogInformation($"Processing message from {queueName}");

		var process = _services.GetRequiredService(queueConfig.QueueType);
		var method = process.GetType().GetMethod("Process");

		if (method == null)
			throw new ArgumentNullException(nameof(method));

		var parameters = method.GetParameters();

		if (parameters.Length != 0)
			Execute(queueName, e.DeliveryTag, method, process, parameters[0].ParameterType, e.Body);
		else
			Execute(queueName, e.DeliveryTag, method, process);
	}

	private async Task Execute(string queueName, ulong tag, MethodInfo method, object process)
	{
		try
		{
			var result = method.Invoke(process, null) as Task;

			if (result != null)
				await result;

			await Ack(tag);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"Error executing queue {queueName}");

			Nack(tag);
		}
	}

	private async Task Execute(string queueName, ulong tag, MethodInfo method, object process, Type bodyType, ReadOnlyMemory<byte> body)
	{
		try
		{
			var data = GetData(queueName, bodyType, body);
			var result = method.Invoke(process, [data]) as Task;

			if (result != null)
				await result;

			_logger.LogInformation($"Message from {queueName} ACK.");
			await Ack(tag);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"Error executing queue {queueName}, NACK");

			Nack(tag);
		}
	}



	private async Task Ack(ulong tag)
	{
		try
		{
			await _channel.BasicAckAsync(
				deliveryTag: tag,
				multiple: false);
		}
		catch (AlreadyClosedException ex)
		{
			Debug.WriteLine(ex);
		}
	}

	private async Task Nack(ulong tag)
	{
		try
		{
			await _channel.BasicNackAsync(
				deliveryTag: tag,
				multiple: false,
				requeue: true);
		}
		catch (AlreadyClosedException ex)
		{
			Debug.WriteLine(ex);
		}
	}

	private object? GetData(string queueName, Type type, ReadOnlyMemory<byte> data)
	{
		var sData = string.Empty;

		try
		{
			sData = Encoding.UTF8.GetString(data.ToArray());

			return JsonConvert.DeserializeObject(sData, type);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"Error parsing queue {queueName} data ({type}): {sData}");

			return null;
		}
	}
}



public static class RabbitMQDependencyInjectionExtensionsClass
{

	public static void AddMessageBusService(this IServiceCollection services, IConfiguration config)
	{
		ConfigureClient(services, config);
		ConfigureChannel(services, config);

		services.AddSingleton<IMessageBus, MessageBus>();
		services.AddTransient<IPublisher, Publisher>();
	}


	private static void ConfigureClient(IServiceCollection services, IConfiguration config)
	{
		services.AddSingleton(provider =>
		{
			var logger = provider.GetService<ILogger>();
			var config = provider.GetService<IConfiguration>();

			logger.LogInformation("Create a RabbitMQ connection.");

			return GetFactory(config)
				.CreateConnectionAsync()
				.Result;
		});
	}

	private static void ConfigureChannel(IServiceCollection services, IConfiguration config)
	{
		services.AddSingleton(provider =>
		{
			var logger = provider.GetService<ILogger>();
			var connection = provider.GetService<IConnection>();

			logger.LogInformation("Create a RabbitMQ channel.");

			return connection
				.CreateChannelAsync()
				.Result;
		});
	}

	private static ConnectionFactory GetFactory(IConfiguration config)
	{
		return new ConnectionFactory
		{
			HostName = config["Services:RabbitMQ:Host"],
			Port = int.Parse(config["Services:RabbitMQ:Port"]),
			UserName = config["Services:RabbitMQ:Username"],
			Password = config["Services:RabbitMQ:Password"]
		};
	}

}
