using IdentityApi.Model;

namespace IdentityApi.Services
{

    public class LoginRefreshToken : ILoginRefreshToken
    {

        public async Task<Token> Login(Login login)
        {
            throw new NotSupportedException();
        }


    }

}
