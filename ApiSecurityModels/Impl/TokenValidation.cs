using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace SecurityModels;

public class TokenValidation
{

    private JwtSecurityTokenHandler TokenHandler => new JwtSecurityTokenHandler();
    public static TokenValidationResult InvalidToken => new TokenValidationResult { IsValid = false };



    public bool IsValid { get; set; }

    public string? UnauthorizedReason { get; set; }

    public string? NameType { get; set; }

    public string? RoleType { get; set; }

    public string? TokenType { get; set; }

    public string? AuthenticationType { get; set; }

    public string? SecurityToken { get; set; }

    public IEnumerable<ClaimValidation> Claims { get; set; } = new List<ClaimValidation>();



    public TokenValidationResult ToResult()
    {
        return new TokenValidationResult
        {
            IsValid = IsValid,
            Exception = !string.IsNullOrEmpty(UnauthorizedReason)
                ? new Exception(UnauthorizedReason) : null,
            ClaimsIdentity = new ClaimsIdentity(
                claims: Claims.Select(x => new Claim(x.Type, x.Value)),
                authenticationType: AuthenticationType,
                nameType: NameType,
                roleType: RoleType
            ),
            SecurityToken = string.IsNullOrEmpty(SecurityToken) 
                ? null : TokenHandler.ReadJwtToken(SecurityToken),
            TokenType = TokenType
        };
    }

}


public class ClaimValidation(string type, string value)
{

    public string Type => type;

    public string Value => value;

}
