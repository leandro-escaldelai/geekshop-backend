using CouponApi.Services;
using ConfigModels;
using CouponApi;

Console.WriteLine("Starting API");

var builder = CreateHostBuilder(args);
var host = builder.Build();

await host.StartAsync();
await host.WaitForShutdownAsync();

Console.WriteLine("End API");



static IHostBuilder CreateHostBuilder(string[] args)
{
	var builder = Host.CreateDefaultBuilder(args);

	builder.ConfigureWebHostDefaults(ConfigureDefaults);
	builder.ConfigureAppConfiguration(ConfigureAppsettings);

	return builder;
}

static void ConfigureDefaults(IWebHostBuilder builder)
{
	builder.UseStartup<Startup>();
}

static void ConfigureAppsettings(HostBuilderContext context, IConfigurationBuilder builder)
{
	var security = ClientLogin.Login();

	builder.AddService(security);
}
