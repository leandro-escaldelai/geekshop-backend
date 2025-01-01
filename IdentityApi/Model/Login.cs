using System.Text.Json.Serialization;

namespace IdentityApi.Model
{

    public class Login
    {

        public string? GrantType { get; set; }

        public string? ClientId { get; set; }

        public string? Username { get; set; }

        public string? Password { get; set; }

        public string? RefreshToken { get; set; }

        public string? ClientSecret { get; set; }

        public IEnumerable<string> Scopes { get; set; } = new List<string>();

    }
    
}
