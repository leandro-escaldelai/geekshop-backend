using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LoggerModels;


public class AppLoggerProvider(
	IConfiguration _config) : ILoggerProvider
{

	private Dictionary<string, ILogger> providers = new Dictionary<string, ILogger>();
	private readonly IConfiguration config = _config;


	public ILogger CreateLogger(string categoryName)
	{
		if (!providers.ContainsKey(categoryName))
			providers.Add(categoryName, new AppLogger(config));

		return providers[categoryName];
	}

	public void Dispose()
	{
	}

}
