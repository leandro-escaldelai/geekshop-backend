namespace IdentityApi.Model
{

    public class Token
    {

        public string? UnauthorizedReason { get; set; }

        public string? AccessToken { get; set; }

        public long? ExpiresIn { get; set; }

        public string? TokenType { get; set; }



        public static Token InvalidLoginData => new Token { UnauthorizedReason = "Invalid Login Data" };
        
        public static Token InvalidGrantType => new Token { UnauthorizedReason = "Invalid Grant Type" };

        public static Token InvalidClient => new Token { UnauthorizedReason = "Invalid Client Id" };

        public static Token InvalidUsernamePassword => new Token { UnauthorizedReason = "Invalid Username or Password" };



        public static class Types
        {

            public const string Bearer = "Bearer";

        }

    }

}
