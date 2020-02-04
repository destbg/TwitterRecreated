using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using System;

namespace Application.Common.ViewModels
{
    public class UserVm : IMapFrom<AppUser>
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public int Following { get; set; }
        public int Followers { get; set; }
        public bool Followed { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Image { get; set; }
        public string Thumbnail { get; set; }
        public bool Verified { get; set; }

        public void Mapping(Profile profile) =>
            profile.CreateMap<AppUser, UserVm>()
                .ForMember(f => f.Username, f => f.MapFrom(s => s.UserName))
                .ForMember(f => f.CreatedOn, f => f.MapFrom(s => s.JoinedOn));
    }

    public class UserShortVm : IMapFrom<AppUser>
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Image { get; set; }

        public void Mapping(Profile profile) =>
            profile.CreateMap<AppUser, UserShortVm>()
                .ForMember(f => f.Username, f => f.MapFrom(s => s.UserName));
    }

    public class UserFollowVm : IMapFrom<AppUser>
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Image { get; set; }
        public bool Followed { get; set; }

        public void Mapping(Profile profile) =>
            profile.CreateMap<AppUser, UserFollowVm>()
                .ForMember(f => f.Username, f => f.MapFrom(s => s.UserName))
                .ForMember(f => f.Followed, f => f.MapFrom(_ => false));
    }
}
