using IdentityApi.Model;
using AutoMapper;

namespace IdentityApi.ValueObject
{

    public class UserVO
    {

        public int? Id { get; set; }

        public string? Name { get; set; }

        public string? Username { get; set; }

        public string? Password { get; set; }

        public bool IsActive { get; set; }

        public IEnumerable<string> Roles { get; set; } = new List<string>();

    }



    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserVO>()
                .ForPath(d => d.Roles, o => o.MapFrom(s => s.Roles.Select(x => x.Role.Name)))
                .ReverseMap()
                .ForPath(d => d.Roles, o => o.MapFrom(s => s.Roles.Select(x => new UserRole { Role = new Role { Name = x } })));
        }

    }

}
