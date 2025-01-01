using IdentityApi.Model;

namespace IdentityApi.Services
{

    public class LoginClientCredentials : ILoginClientCredentials
    {

        public async Task<Token> Login(Login login)
        {
            throw new NotSupportedException();
        }

    }

}
