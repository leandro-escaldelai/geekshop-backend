namespace IdentityApi.Model
{

    public class ClientGrantType
    {

        public int? Id { get; set; }

        public Client? Client { get; set; }

        public GrantType? GrantType { get; set; }

    }

}
