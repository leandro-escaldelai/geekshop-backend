using IdentityApi.Model;

namespace IdentityApi.Services
{

    public interface ITokenService
    {

        TokenValidation Validate(HttpRequest request);

    }

}
