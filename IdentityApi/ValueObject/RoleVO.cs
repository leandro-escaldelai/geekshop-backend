using IdentityApi.Model;
using AutoMapper;

namespace IdentityApi.ValueObject
{
    public class RoleVO
    {

        public int? Id { get; set; }
        public string? Name { get; set; }

    }



    public class RoleProfile : Profile
    {
    
        public RoleProfile()
        {
            CreateMap<Role, RoleVO>().ReverseMap();
        }
    
    }

}
