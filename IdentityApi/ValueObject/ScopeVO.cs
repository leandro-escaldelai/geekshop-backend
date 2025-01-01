using IdentityApi.Model;
using AutoMapper;

namespace IdentityApi.ValueObject
{
    public class ScopeVO
    {

        public int? Id { get; set; }
        public string? Name { get; set; }

    }



    public class ScopeProfile : Profile
    {
    
        public ScopeProfile()
        {
            CreateMap<Scope, ScopeVO>().ReverseMap();
        }
    
    }

}
