using System;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.ViewModels
{
    public class BookmarkVm : IMapFrom<Bookmark>
    {
        public long Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public PostVm Post { get; set; }

        public void Mapping(Profile profile) =>
            profile.CreateMap<Bookmark, BookmarkVm>()
                .ForMember(f => f.Post, f => f.MapFrom(s => s.Post));
    }
}
