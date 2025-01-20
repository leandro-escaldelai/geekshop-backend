using Microsoft.EntityFrameworkCore;
using IdentityApi.Repository;
using IdentityApi.Model;

namespace IdentityApi.Services
{

    public class LoginPassword(
        IConfiguration _configuration,
        Context _context) : ILoginPassword
    {

        private readonly string key = _configuration["SecurityKey"]
            ?? throw new Exception("Invalid Secret Key.");
        private readonly TimeSpan expiration = TimeSpan.Parse(_configuration["Token:Password:Expiration"] ?? "04:00:00");

		public async Task<Token> Login(Login login)
        {
            // Validate client
            var client = await GetClient(login);
            if (client == null)
                return Token.InvalidClient;

            // Validate user
            var user = await GetUser(login);
            if (user == null || !CheckPassword(login, user))
                return Token.InvalidUsernamePassword;

            var roles = await GetUserRoles(user, client);
            var scopes = await GetClientScopes(client);

			return TokenService.Create(key, user, client, roles, scopes, expiration);
        }



        private async Task<Client?> GetClient(Login login)
        {
            return await _context.Set<Client>()
                .Include(x => x.GrantTypes).ThenInclude(x => x.GrantType)
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ClientId == login.ClientId);
        }

        private async Task<User?> GetUser(Login login)
        {
            return await _context.Set<User>()
                .AsNoTracking()
                .SingleOrDefaultAsync(x =>
                    x.Username == login.Username);
        }

        private bool CheckPassword(Login login, User user)
        {
            if (string.IsNullOrEmpty(login.Password))
                return false;

            return user.Password == login.Password.Sha512();
        }

        private async Task<IEnumerable<Role>> GetUserRoles(User user, Client client)
        {
            return await _context.Set<UserRole>()
                .Include(x => x.Role)
                .AsNoTracking()
                .Where(x =>
                    x.User.Id == user.Id &&
                    x.Client.ClientId == client.ClientId)
                .Select(x => x.Role)
                .ToListAsync();
        }

        private async Task<IEnumerable<Scope>> GetClientScopes(Client client)
        {
            return await _context.Set<ClientScope>()
                .Include(x => x.Scope)
                .AsNoTracking()
                .Where(x => x.Client.Id == client.Id)
                .Select(x => x.Scope)
                .ToListAsync();
        }

    }

}
