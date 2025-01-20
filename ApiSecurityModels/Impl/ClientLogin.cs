using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using ClientModels;
using ConfigModels;

namespace CouponApi.Services
{

	public static class ClientLogin
	{

		public static AuthenticationHeaderValue Login()
		{
			var config = Configuration
				.GetJsonFileValues("Id", "Secret", "Services:Identity");

			return Login(
				config["Services:Identity"], 
				config["Id"], 
				config["Secret"]);
		}


		public static AuthenticationHeaderValue Login(string? baseUrl, string? clientId, string? clientSecret)
		{
			using (var http = new HttpClient())
			{
				var token = http.PostUrlEncoded<JObject>($"{baseUrl}/login",
				[
					new KeyValuePair<string, string?>("grant_type", "client_credentials"),
					new KeyValuePair<string, string?>("client_id", clientId),
					new KeyValuePair<string, string?>("client_secret", clientSecret)

				]).Result;

				return new AuthenticationHeaderValue("Bearer", 
					token.Value<string>("access_token"));
			}
		}

	}

}
