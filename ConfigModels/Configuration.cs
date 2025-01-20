using Microsoft.Extensions.Configuration;

namespace ConfigModels;

public static class Configuration
{

	public static IConfiguration GetJsonFile(string name = "appsettings.json")
	{
		var builder = new ConfigurationBuilder();

		builder.AddJsonFile();

		return builder.Build();	
	}

	public static string? GetJsonFileValue(string key)
	{
		return GetJsonFile("appsettings.json")[key];
	}

	public static IDictionary<string, string?> GetJsonFileValues(params string[] keys)
	{
		var result = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
		var config = GetJsonFile("appsettings.json");

		foreach (var key in keys)
			result.Add(key, config[key]);

		return result;
	}

	public static IConfiguration GetEnv()
	{
		var builder = new ConfigurationBuilder();

		builder.AddEnvironmentVariables();

		return builder.Build();	
	}

	public static string? GetEnvValue(string key)
	{
		return GetEnv()[key];
	}

	public static IDictionary<string, string?> GetEnvValues(params string[] keys)
	{
		var result = new Dictionary<string, string?>();
		var config = GetEnv();

		foreach (var key in keys)
			result.Add(key, config[key]);

		return result;
	}

}
