using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Users.Command.EditUserProfile
{
    public class EditUserProfileResponse : IMapFrom<AppUser>
    {
        public string Image { get; set; }
        public string Thumbnail { get; set; }

        public void Mapping(Profile profile) =>
            profile.CreateMap<AppUser, EditUserProfileResponse>()
                .ForMember(f => f.Image, f => f.MapFrom(s => s.Image))
                .ForMember(f => f.Thumbnail, f => f.MapFrom(s => s.Thumbnail));
    }
}
