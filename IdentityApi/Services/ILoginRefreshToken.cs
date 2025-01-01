using IdentityApi.Model;

namespace IdentityApi.Services
{

    public interface ILoginRefreshToken
    {

        Task<Token> Login(Login login);


    }

}
