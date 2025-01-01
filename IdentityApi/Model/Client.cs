namespace IdentityApi.Model
{

    public class Client
    {

        public int? Id { get; set; }

        public string? Name { get; set; }

        public string? ClientId { get; set; }

        public string? ClientSecret { get; set; }

        public IEnumerable<ClientGrantType> GrantTypes { get; set; } = new List<ClientGrantType>();

        public IEnumerable<ClientScope> Scopes { get; set; } = new List<ClientScope>();

    }

}
