using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using IdentityApi.Model;
using ConfigModels;
using System.Text;
using IdentityApi.ValueObject;

namespace IdentityApi.Services
{

    public class TokenService : ITokenService
    {

        private JwtSecurityTokenHandler TokenHandler => new JwtSecurityTokenHandler();

        public TokenValidation Validate(HttpRequest request)
        {
            var authHeader = $"{request.Headers["Authorization"]}";

            if (string.IsNullOrEmpty(authHeader))
                return TokenValidation.AuthorizationHeaderMissing;

            if (authHeader.ToLower().StartsWith("bearer ") == false)
                return TokenValidation.InvalidTokenType;

            authHeader = authHeader
                .Replace("Bearer ", string.Empty)
                .Replace("bearer ", string.Empty)
                .Replace("BEARER ", string.Empty);

            if (string.IsNullOrEmpty(authHeader))
                return TokenValidation.InvalidToken;

            var token = TokenHandler.ReadJwtToken(authHeader);

            if (token == null)
                return TokenValidation.InvalidToken;

            if (token.ValidFrom > DateTime.UtcNow || token.ValidTo < DateTime.UtcNow)
                return TokenValidation.ExpiredToken;

            return new TokenValidation
            {
                IsValid = true,
                NameType = ClaimTypes.Name,
                RoleType = ClaimTypes.Role,
                TokenType = "JWT",
                AuthenticationType = "Bearer",
                SecurityToken = authHeader,
                Claims = token.Claims.Select(x => new ClaimValidation(x.Type, x.Value)),
            };
        }

        public static Token Create(string key, User user, Client client, IEnumerable<Role> roles, IEnumerable<Scope> scopes, TimeSpan expireTime)
        {
			var expires = DateTime.UtcNow.Add(expireTime);

			// Generate JWT token
			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = GetClaimsIdentity(user, client, roles, scopes),
				Expires = expires,
				SigningCredentials = GetCredentials(key)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			var tokenString = tokenHandler.WriteToken(token);

			return new Token
			{
				AccessToken = tokenString,
				ExpiresIn = GetExpiresIn(expires),
				TokenType = Token.Types.Bearer
			};

        }

        public static Token Create(string key, Client client, IEnumerable<Scope> scopes, TimeSpan expireTime)
        {
			var expires = DateTime.UtcNow.Add(expireTime);

			// Generate JWT token
			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = GetClaimsIdentity(client, scopes),
				Expires = expires,
				SigningCredentials = GetCredentials(key)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			var tokenString = tokenHandler.WriteToken(token);

			return new Token
			{
				AccessToken = tokenString,
				ExpiresIn = GetExpiresIn(expires),
				TokenType = Token.Types.Bearer
			};
        }

		public static string CreateSelf()
		{
			var expires = DateTime.UtcNow.AddMinutes(10);
			var preConfig = Configuration.GetJsonFile();
			var key = preConfig["SecurityKey"] ?? throw new ArgumentNullException("SecurityKey");
			var scopes = new Scope[] { new Scope { Name = "config:get" } };

			// Generate JWT token
			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = GetClaimsIdentity(scopes),
				Expires = expires,
				SigningCredentials = GetCredentials(key)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);
		}



		private static ClaimsIdentity GetClaimsIdentity(User user, Client client, IEnumerable<Role> roles, IEnumerable<Scope> scopes)
		{
			var subject = new ClaimsIdentity();

			subject.AddClaim(new Claim(ClaimTypes.NameIdentifier, $"{user.Id}"));
			subject.AddClaim(new Claim(ClaimTypes.Name, $"{user.Username}"));
			subject.AddClaim(new Claim(ClaimTypes.GivenName, $"{user.Name}"));
			subject.AddClaim(new Claim("ClientId", $"{client.ClientId}"));
			subject.AddClaims(GetClaims(roles));
			subject.AddClaims(GetClaims(scopes));

			return subject;
		}

		private static ClaimsIdentity GetClaimsIdentity(Client client, IEnumerable<Scope> scopes)
		{
			var subject = new ClaimsIdentity();

			subject.AddClaim(new Claim(ClaimTypes.NameIdentifier, $"{client.Id}"));
			subject.AddClaim(new Claim(ClaimTypes.Name, $"{client.Name}"));
			subject.AddClaim(new Claim(ClaimTypes.GivenName, $"{client.Name}"));
			subject.AddClaim(new Claim("ClientId", $"{client.ClientId}"));
			subject.AddClaims(GetClaims(scopes));

			return subject;
		}

		private static ClaimsIdentity GetClaimsIdentity(IEnumerable<Scope> scopes)
		{
			var subject = new ClaimsIdentity();

			subject.AddClaim(new Claim(ClaimTypes.NameIdentifier, "2"));
			subject.AddClaim(new Claim(ClaimTypes.Name, "Identity API"));
			subject.AddClaim(new Claim(ClaimTypes.GivenName, "Identity API"));
			subject.AddClaims(GetClaims(scopes));

			return subject;
		}

		private static IEnumerable<Claim> GetClaims(IEnumerable<Role> roles)
		{
			var claims = new List<Claim>();

			foreach (var role in roles)
			{
				if (string.IsNullOrEmpty(role.Name))
					continue;

				claims.Add(new Claim(ClaimTypes.Role, role.Name));
			}

			return claims;
		}

		private static IEnumerable<Claim> GetClaims(IEnumerable<Scope> scopes)
		{
			var claims = new List<Claim>();

			foreach (var scope in scopes)
			{
				if (string.IsNullOrEmpty(scope.Name))
					continue;

				claims.Add(new Claim(ClaimTypes.Role, scope.Name));
			}

			return claims;
		}

		private static SigningCredentials GetCredentials(string key)
		{
			var bKey = Encoding.ASCII.GetBytes(key);
			var secret = new SymmetricSecurityKey(bKey);
			var credentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);

			return credentials;
		}

		private static long GetExpiresIn(DateTime date)
		{
			var time = date.Subtract(DateTime.Now);
			var seconds = time.TotalSeconds;

			return Convert.ToInt64(seconds);
		}

	}


	public static class TokenServiceExtensions
    {
        public static IServiceCollection AddTokenService(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }

}
