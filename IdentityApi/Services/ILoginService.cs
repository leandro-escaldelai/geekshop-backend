using IdentityApi.ValueObject;
using IdentityApi.Model;

namespace IdentityApi.Services
{

    public interface ILoginService
    {

        Task<Token> Login(LoginVO login);

    }

}
