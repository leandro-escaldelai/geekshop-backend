using System.Net.Http.Headers;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Net;

namespace ClientModels;

public static class HttpClientExtensions
{

    public static async Task<T?> Get<T>(this HttpClient http, string url, AuthenticationHeaderValue? authorization = null)
    {
        var message = GetMessage(HttpMethod.Get, url, null, authorization);

        return await SendMessage<T>(http, message);
    }

    public static async Task<T?> Post<T>(this HttpClient http, string url, object body, AuthenticationHeaderValue? authorization = null)
    {
        var message = GetMessage(HttpMethod.Post, url, body, authorization);

        return await SendMessage<T>(http, message);
    }

    public static async Task<T?> PostUrlEncoded<T>(this HttpClient http, string url, IEnumerable<KeyValuePair<string, string?>> body, AuthenticationHeaderValue? authorization = null)
    {
        var message = GetUrlEncodedMessage(HttpMethod.Post, url, body, authorization);

        return await SendMessage<T>(http, message);
    }

    public static async Task<T?> Put<T>(this HttpClient http, string url, object body, AuthenticationHeaderValue? authorization = null)
    {
        var message = GetMessage(HttpMethod.Put, url, body, authorization);

        return await SendMessage<T>(http, message);
    }

    public static async Task Delete(this HttpClient http, string url, AuthenticationHeaderValue? authorization = null)
    {
        var message = GetMessage(HttpMethod.Delete, url, null, authorization);

        await SendMessage(http, message);
    }



    private static HttpRequestMessage GetMessage(
        HttpMethod method, 
        string url, object? body = null, 
        AuthenticationHeaderValue? authorization = null)
    {
        var message = new HttpRequestMessage(method, url);

        if (authorization != null)
            message.Headers.Authorization = authorization;

        if (body != null)
            message.Content = JsonContent.Create(body);

        return message;
    }

    private static HttpRequestMessage GetUrlEncodedMessage(
        HttpMethod method, 
        string url, object? body = null, 
        AuthenticationHeaderValue? authorization = null)
    {
        var message = new HttpRequestMessage(method, url);

        if (authorization != null)
            message.Headers.Authorization = authorization;

        var dic = body as IEnumerable<KeyValuePair<string, string?>>;

		if (dic != null)
            message.Content = new FormUrlEncodedContent(dic);

        return message;
    }

    private static async Task<T?> SendMessage<T>(HttpClient http, HttpRequestMessage message)
    {
        var response = await SendMessage(http, message);

        if (response.StatusCode == HttpStatusCode.NoContent)
            return default;

        var sContent = await response.Content.ReadAsStringAsync();
        var content = JsonConvert.DeserializeObject<T>(sContent);

			return content;
    }

    private static async Task<HttpResponseMessage> SendMessage(HttpClient http, HttpRequestMessage message)
    {
        var response = await http.SendAsync(message);

        if (!response.IsSuccessStatusCode)
            throw new Exception($"{response.StatusCode} - {response.ReasonPhrase}");

        return response;
    }

}
