using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using IdentityApi.Repository;
using IdentityApi.Model;
using System.Security.Claims;
using System.Text;

namespace IdentityApi.Services
{

    public class LoginPassword(
        IConfiguration _configuration,
        Context _context) : ILoginPassword
    {

        private readonly string _secretKey = _configuration["SecretKey"]
            ?? throw new Exception("Invalid Secret Key.");

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

            return await GetToken(user, client);
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

        private async Task<Token> GetToken(User user, Client client)
        {
            var expires = DateTime.UtcNow.AddHours(1);

            // Generate JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = await GetSubject(user, client),
                Expires = expires,
                SigningCredentials = GetCredentials()
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

        private async Task<ClaimsIdentity> GetSubject(User user, Client client)
        {
            var subject = new ClaimsIdentity();

            subject.AddClaim(new Claim(ClaimTypes.NameIdentifier, $"{user.Id}"));
            subject.AddClaim(new Claim(ClaimTypes.Name, $"{user.Username}"));
            subject.AddClaim(new Claim(ClaimTypes.GivenName, $"{user.Name}"));
            subject.AddClaim(new Claim("ClientId", $"{client.ClientId}"));
            subject.AddClaims(await GetRoles(user, client));
            subject.AddClaims(await GetScopes(client));

            return subject;
        }

        private async Task<IEnumerable<Claim?>> GetRoles(User user, Client client)
        {
            var permissions = await GetUserRoles(user, client);
            var roles = new List<Claim>();

            foreach (var permission in permissions)
            {
                if (string.IsNullOrEmpty(permission.Name))
                    continue;

                roles.Add(new Claim(ClaimTypes.Role, permission.Name));
            }

            return roles;
        }

        private async Task<IEnumerable<Claim?>> GetScopes(Client client)
        {
            var permissions = await GetClientScopes(client);
            var roles = new List<Claim>();

            foreach (var permission in permissions)
            {
                if (string.IsNullOrEmpty(permission.Name))
                    continue;

                roles.Add(new Claim("scope", permission.Name));
            }

            return roles;
        }

        private SigningCredentials GetCredentials()
        {
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var secret = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);

            return credentials;
        }

        private async Task<IEnumerable<Role?>> GetUserRoles(User user, Client client)
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

        private async Task<IEnumerable<Scope?>> GetClientScopes(Client client)
        {
            return await _context.Set<ClientScope>()
                .Include(x => x.Scope)
                .AsNoTracking()
                .Where(x => x.Client.Id == client.Id)
                .Select(x => x.Scope)
                .ToListAsync();
        }

        private long GetExpiresIn(DateTime date)
        {
            var time = date.Subtract(DateTime.Now);
            var seconds = time.TotalSeconds;

            return Convert.ToInt64(seconds);
        }


    }

}
