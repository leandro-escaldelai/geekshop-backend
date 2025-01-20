using Microsoft.EntityFrameworkCore;
using IdentityApi.Repository;
using IdentityApi.Model;

namespace IdentityApi.Services
{

    public class LoginClientCredentials(
		IConfiguration _configuration,
		Context _context) : ILoginClientCredentials
    {

		private readonly string key = _configuration["SecurityKey"]
	        ?? throw new Exception("Invalid Secret Key.");
		private readonly TimeSpan expiration = TimeSpan.Parse(_configuration["Token:ClientCredentials:Expiration"] ?? "1.00:00:00");

		public async Task<Token> Login(Login login)
        {
			// Validate client
			var client = await GetClient(login);
			if (client == null || !CheckSecret(login, client))
				return Token.InvalidClient;

			var scopes = await GetScopes(client);

			return TokenService.Create(key, client, scopes, expiration);
		}



		private async Task<Client?> GetClient(Login login)
		{
			return await _context.Set<Client>()
				.Include(x => x.GrantTypes).ThenInclude(x => x.GrantType)
				.AsNoTracking()
				.SingleOrDefaultAsync(x => x.ClientId == login.ClientId);
		}

		private bool CheckSecret(Login login, Client client)
		{
			if (string.IsNullOrEmpty(login.ClientSecret))
				return false;

			return client.ClientSecret == login.ClientSecret.Sha512();
		}

		private async Task<IEnumerable<Scope>> GetScopes(Client client)
		{
			return await _context.Set<ClientScope>()
				.Include(x => x.Scope)
				.Where(x => x.Client.Id == client.Id)
				.Select(x => x.Scope)
				.AsNoTracking()
				.ToListAsync();
		}

	}

}
