using Microsoft.AspNetCore.Mvc;
using IdentityApi.Model;
using AutoMapper;

namespace IdentityApi.ValueObject
{

    public class LoginVO
    {

        [ModelBinder(Name = "grant_type")]
        public string? GrantType { get; set; }

        [ModelBinder(Name = "client_id")]
        public string? ClientId { get; set; }

        public string? Username { get; set; }

        public string? Password { get; set; }

        [ModelBinder(Name = "refresh_token")]
        public string? RefreshToken { get; set; }

        [ModelBinder(Name = "client_secret")]
        public string? ClientSecret { get; set; }

        [ModelBinder(Name = "scope")]
        public string? Scope { get; set; }

    }



    public class LoginProfile : Profile
    {

        public LoginProfile()
        {
            CreateMap<LoginVO, Login>()
                .ForPath(d => d.Scopes, o => o.MapFrom(s => GetScopes(s)));
        }



        private IEnumerable<string> GetScopes(LoginVO data)
        {
            var scopes = new List<string>();

            if (!string.IsNullOrWhiteSpace(data.Scope))
                scopes.AddRange(data.Scope.Split(' '));

            return scopes;
        }

    }

}
