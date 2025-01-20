using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace IdentityApi
{

    public static class ServiceExtensions
    {

        public static string? Sha512(this string? input)
        {
            if (input == null) return null;
            if (input == "") return "";

            var sha = SHA512.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha.ComputeHash(bytes);
            
            return Convert.ToBase64String(hash);
        }

        public static void ConfigureAuthentication(this WebApplicationBuilder builder)
        {
            var services = builder.Services;
            var configuration = builder.Configuration;

            var sKey = configuration["SecretKey"];

            if (string.IsNullOrEmpty(sKey))
                throw new Exception("Key not configured.");

            var key = Encoding.ASCII.GetBytes(sKey);

            var authentication = services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });

            authentication.AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                options.Authority = null;
            });
        }


    }

}
