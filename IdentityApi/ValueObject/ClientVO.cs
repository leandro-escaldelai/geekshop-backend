using IdentityApi.Model;
using AutoMapper;

namespace IdentityApi.ValueObject
{

    public class ClientVO
    {

        public int? Id { get; set; }

        public string? Name { get; set; }

        public string? ClientId { get; set; }

        public string? ClientSecret { get; set; }

        public IEnumerable<string> GrantTypes { get; set; } = new List<string>();

        public IEnumerable<string> Scopes { get; set; } = new List<string>();

    }


    public class ClientProfile : Profile
    {

        public ClientProfile()
        {
            CreateMap<Client, ClientVO>()
                .ForPath(d => d.GrantTypes, o => o.MapFrom(s => s.GrantTypes.Select(x => x.GrantType.Name)))
                .ForPath(d => d.Scopes, o => o.MapFrom(s => s.Scopes.Select(x => x.Scope.Name)))
                .ReverseMap()
                .ForPath(d => d.GrantTypes, o => o.MapFrom(s => s.GrantTypes.Select(x => new ClientGrantType { GrantType = new GrantType { Name = x } })))
                .ForPath(d => d.Scopes, o => o.MapFrom(s => s.Scopes.Select(x => new ClientScope { Scope = new Scope { Name = x } })));
        }

    }

}
