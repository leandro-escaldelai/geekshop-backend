using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Text;

namespace MessageBus;

public class Publisher(
    IConnection _connection) : IPublisher
{

    public async Task Publish<T>(Message<T> message, string queueName, string exchange = "")
    {
        using (var channel = await _connection.CreateChannelAsync())
        {
            await channel.BasicPublishAsync(
                exchange: exchange,
                routingKey: queueName,
                mandatory: false,
                basicProperties: new BasicProperties(),
                body: Serialize(message));
        }
    }

    public async Task Publish(Message message, string queueName, string exchange = "")
    {
        using (var channel = await _connection.CreateChannelAsync())
        {
            await channel.BasicPublishAsync(
                exchange: exchange,
                routingKey: queueName,
                mandatory: false,
                basicProperties: new BasicProperties(),
                body: Serialize(message));
        }
    }


    private ReadOnlyMemory<byte> Serialize(object data)
    {
        var json = JsonConvert.SerializeObject(data);

        return Encoding.UTF8.GetBytes(json);
    }

}



public static class MessageBusExtensions
{

    public static void AddPublisher(this IServiceCollection services)
    {
        services.AddSingleton(provider =>
        {
            var configuration = provider.GetService<IConfiguration>() ?? throw new ArgumentNullException("configuration");
            var factory = new ConnectionFactory
            {
                HostName = configuration["Services:RabbitMQ:Host"] ?? "localhost",
                Port = Convert.ToInt32(configuration["Services:RabbitMQ:Port"] ?? "5672"),
                UserName = configuration["Services:RabbitMQ:Username"] ?? "",
                Password = configuration["Services:RabbitMQ:Password"] ?? "",
            };

            return factory.CreateConnectionAsync().Result;
        });

        services.AddScoped<IPublisher, Publisher>();
    }

}
