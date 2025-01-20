using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;



namespace ConfigModels;

public static class ConfigExtensions
{

	public static IConfigurationBuilder AddJsonFile(this IConfigurationBuilder builder)
	{
		var path = Path.Combine(Environment.CurrentDirectory, "appsettings.json");

		builder.AddJsonFile(path);

		return builder;
	}

	public static void AddService(this IConfigurationBuilder builder, AuthenticationHeaderValue securityContext)
	{
		builder.Add(new ConfigSource(securityContext));
	}

}
