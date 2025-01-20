using IdentityApi;

Console.WriteLine("Start API");

var builder = CreateHostBuilder(args);
var host = builder.Build();

await host.StartAsync();
await host.WaitForShutdownAsync();

Console.WriteLine("End API");



static IHostBuilder CreateHostBuilder(string[] args)
{
	var builder = Host.CreateDefaultBuilder(args);

	builder.ConfigureWebHostDefaults(ConfigureDefaults);

	return builder;
}

static void ConfigureDefaults(IWebHostBuilder builder)
{
	builder.UseStartup<Startup>();
}
