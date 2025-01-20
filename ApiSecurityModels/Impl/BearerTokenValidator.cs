using Microsoft.IdentityModel.Tokens;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net;

namespace SecurityModels;

public class BearerTokenValidator(
    IConfiguration _configuration) : TokenHandler
{

    private readonly string validateUrl = $"{_configuration["Services:Identity"]}/token/validate";

    public async override Task<TokenValidationResult> ValidateTokenAsync(string token, TokenValidationParameters validationParameters)
    {
        var message = new HttpRequestMessage(HttpMethod.Get, validateUrl);

        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using (var client = new HttpClient())
        {
            var response = await client.SendAsync(message);

            return (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.BadRequest)
                ? await GetResult(response) : TokenValidation.InvalidToken;
        }
    }

    private async Task<TokenValidationResult> GetResult(HttpResponseMessage response)
    {
        var result = await response.Content.ReadAsStringAsync();
        var validation = JsonConvert.DeserializeObject<TokenValidation>(result);

        return validation != null ? validation.ToResult() : TokenValidation.InvalidToken;
    }

}



public static class SecurityExtensions
{

    public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<TokenHandler, BearerTokenValidator>();
        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenHandlers.Clear();
                options.TokenHandlers.Add(new BearerTokenValidator(configuration));
            });
    }

}
