using IdentityApi.Model;

namespace IdentityApi.Services
{

    public interface ILoginPassword
    {

        Task<Token> Login(Login login);

    }

}
