using IdentityApi.Model;
using AutoMapper;

namespace IdentityApi.ValueObject
{

    public class GrantTypeVO
    {

        public int? Id { get; set; }

        public string? Name { get; set; }

    }

    public class GrantTypeProfile : Profile
    {

        public GrantTypeProfile()
        {
            CreateMap<GrantType, GrantTypeVO>()
                .ReverseMap();
        }

    }

}
