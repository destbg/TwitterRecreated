using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.ViewModels
{
    public class AuthVm
    {
        public string Token { get; set; }
        public AuthUserVm User { get; set; }
    }

    public class AuthUserVm : IMapFrom<AppUser>
    {
        public string Username { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }

        public void Mapping(Profile profile) =>
            profile.CreateMap<AppUser, AuthUserVm>()
                .ForMember(f => f.Username, f => f.MapFrom(s => s.UserName));
    }
}
