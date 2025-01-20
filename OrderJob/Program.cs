using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OrderJob.Repository;
using OrderJob.Services;
using OrderJob.Queues;
using MessageBus;
using LoggerModels;



var services = GetServices();

await services.GetRequiredService<IMessageBus>()
	.ConsumeQueue<CreatedOrderQueue>(CreatedOrderQueue.Name)
	.Run();



IServiceProvider GetServices()
{
	var services = new ServiceCollection();
	var configuration = GetConfiguration();

	AddLogger(services, configuration);
	services.AddSingleton(configuration);
	services.AddRepositoryContext();
	services.AddTransient<IOrderService, OrderService>();
	services.AddTransient<CreatedOrderQueue>();
	services.AddMessageBusService(configuration);

	return services.BuildServiceProvider();
}

IConfiguration GetConfiguration()
{
	var config = new ConfigurationBuilder();

	config.AddEnvironmentVariables();
	config.AddJsonFile(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"
		? "appsettings.Development.json"
		: "appsettings.json");

	return config.Build();
}

void AddLogger(IServiceCollection services, IConfiguration configuration)
{
	var loggerFactory = LoggerFactory.Create(builder =>
	{
		builder.ClearProviders();
		builder.AddProvider(new AppLoggerProvider(configuration));
	});

	var logger = loggerFactory.CreateLogger("OrderJob");

	services.AddSingleton(logger);
}
