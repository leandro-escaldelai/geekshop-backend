using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;



namespace ConfigModels;

public class ConfigSource(
	AuthenticationHeaderValue security) : IConfigurationSource
{

	private readonly IConfigurationProvider provider = new ConfigProvider(security);



	public IConfigurationProvider Build(IConfigurationBuilder builder) => provider;

}
