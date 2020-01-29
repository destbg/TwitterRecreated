using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using System;
using System.Linq;

namespace Application.Common.ViewModels
{
    public class PostVm : IMapFrom<Post>
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public string Video { get; set; }
        public string[] Images { get; set; }
        public UserShortVm User { get; set; }
        public DateTime PostedOn { get; set; }
        public bool IsLiked { get; set; }
        public int Likes { get; set; }
        public int Comments { get; set; }
        public int Reposts { get; set; }
        public PollVm[] Poll { get; set; }
        public DateTime PollEnd { get; set; }
        public string[] Reposters { get; set; }
        public string[] Likers { get; set; }
        public PostShortVm Repost { get; set; }
        public PostReplyVm Reply { get; set; }

        public void Mapping(Profile profile) =>
            profile.CreateMap<Post, PostVm>()
                .ForMember(f => f.PostedOn, f => f.MapFrom(s => s.CreatedOn))
                .ForMember(f => f.Images, f => f.MapFrom(s => s.Images.Select(f => f.Image)))
                .ForMember(f => f.User, f => f.MapFrom(s => s.User))
                .ForMember(f => f.Poll, f => f.MapFrom(s => s.Poll))
                .ForMember(f => f.Repost, f => f.MapFrom(s => s.Repost))
                .ForMember(f => f.Reply, f => f.MapFrom(s => s.Reply));
    }

    public class PostShortVm : IMapFrom<Post>
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public UserShortVm User { get; set; }
        public DateTime PostedOn { get; set; }

        public void Mapping(Profile profile) =>
            profile.CreateMap<Post, PostShortVm>()
                .ForMember(f => f.PostedOn, f => f.MapFrom(s => s.CreatedOn))
                .ForMember(f => f.User, f => f.MapFrom(s => s.User));
    }

    public class RepostVm : IMapFrom<Repost>
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public UserShortVm User { get; set; }
        public DateTime PostedOn { get; set; }

        public void Mapping(Profile profile) =>
            profile.CreateMap<Repost, RepostVm>()
                .ForMember(f => f.PostedOn, f => f.MapFrom(s => s.CreatedOn))
                .ForMember(f => f.User, f => f.MapFrom(s => s.User));
    }

    public class PostReplyVm : IMapFrom<Post>
    {
        public string Username { get; set; }
        public long PostId { get; set; }
        public string Content { get; set; }

        public void Mapping(Profile profile) =>
            profile.CreateMap<Post, PostReplyVm>()
                .ForMember(f => f.PostId, f => f.MapFrom(s => s.ReplyId))
                .ForMember(f => f.Username, f => f.MapFrom(s => s.User.UserName));
    }
}
