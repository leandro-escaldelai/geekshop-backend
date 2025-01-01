using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using IdentityApi.ValueObject;
using IdentityApi.Repository;
using System.Security.Claims;
using IdentityApi.Model;
using System.Text;
using AutoMapper;

namespace IdentityApi.Services
{

    public class LoginService(
        IMapper _mapper,
        ILoginPassword _loginPassword,
        ILoginClientCredentials _loginClientCredentials,
        ILoginRefreshToken _loginRefreshToken) : ILoginService
    {

        public async Task<Token> Login(LoginVO login)
        {
            var data = _mapper.Map<Login>(login);

            if (data == null)
                return Token.InvalidLoginData;

            return login.GrantType switch
            {
                GrantType.Password => await _loginPassword.Login(data),
                GrantType.ClientCredentials => await _loginClientCredentials.Login(data),
                GrantType.RefreshToken => await _loginRefreshToken.Login(data),
                _ => Token.InvalidGrantType
            };
        }

    }


    public static class LoginServiceExtensions
    {
        public static IServiceCollection AddLoginService(this IServiceCollection services)
        {
            services.AddScoped<ILoginPassword, LoginPassword>();
            services.AddScoped<ILoginClientCredentials, LoginClientCredentials>();
            services.AddScoped<ILoginRefreshToken, LoginRefreshToken>();
            services.AddScoped<ILoginService, LoginService>();

            return services;
        }
    }   

}
