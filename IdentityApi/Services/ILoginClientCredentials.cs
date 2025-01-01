using IdentityApi.Model;

namespace IdentityApi.Services
{

    public interface ILoginClientCredentials
    {

        Task<Token> Login(Login login);


    }

}
