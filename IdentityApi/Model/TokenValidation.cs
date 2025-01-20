using System.Security.Claims;

namespace IdentityApi.Model
{

    public class TokenValidation
    {

        public bool IsValid { get; set; }

        public string? UnauthorizedReason { get; set; }

        public string? NameType { get; set; }

        public string? RoleType { get; set; }

        public string? TokenType { get; set; }

        public string? AuthenticationType { get; set; }

        public string? SecurityToken { get; set; }

        public IEnumerable<ClaimValidation> Claims { get; set; } = new List<ClaimValidation>();



        public static TokenValidation AuthorizationHeaderMissing => new TokenValidation
            { IsValid = false, UnauthorizedReason = "Authorization header missing." };

        public static TokenValidation InvalidTokenType => new TokenValidation
            { IsValid = false, UnauthorizedReason = "Invalid token type, expected Bearer." };

        public static TokenValidation InvalidToken => new TokenValidation
            { IsValid = false, UnauthorizedReason = "Invalid token data." };

        public static TokenValidation ExpiredToken => new TokenValidation
            { IsValid = false, UnauthorizedReason = "Token is expired." };

    }


    public class ClaimValidation(
        string type, 
        string value)
    {

        public string Type { get; set; } = type;

        public string Value { get; set; } = value;

    }

}
