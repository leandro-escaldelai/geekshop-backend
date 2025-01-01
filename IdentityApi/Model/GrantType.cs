namespace IdentityApi.Model
{

    public class GrantType
    {

        public int? Id {  get; set; }

        public string? Name { get; set; }


        public const string Password = "password";

        public const string ClientCredentials = "client_credentials";
        
        public const string RefreshToken = "refresh_token";

    }

}
