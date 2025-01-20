using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using ConfigModels.Model;
using ClientModels;



namespace ConfigModels;

public class ConfigProvider(
	AuthenticationHeaderValue token) : ConfigurationProvider
{

	public override void Load()
	{
		var baseUrl = GetUrl();
		var url = $"{baseUrl}/config";

		using (var client = new HttpClient())
		{
			var data = client
				.Get<IEnumerable<ConfigVO>>(url, token)
				.Result;

			if (data == null)
				data = new List<ConfigVO>();

			Data = data.ToDictionary(x => x.Name ?? "", x => x.Value);
		}
	}


	private string GetUrl()
	{
		return
			Configuration.GetEnvValue("Services:Config") ??
			Configuration.GetJsonFileValue("Services:Config") ??
			throw new ArgumentNullException("Config API Url");
	}

}
