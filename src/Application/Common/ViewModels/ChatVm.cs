using System.Collections.Generic;
using System.Linq;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.ViewModels
{
    public class ChatVm : IMapFrom<Chat>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public IEnumerable<ChatUserVm> Users { get; set; }
        public string LastMessage { get; set; }
        public bool NewMessage { get; set; }
        public bool IsGroup { get; set; }

        public void Mapping(Profile profile) =>
            profile.CreateMap<Chat, ChatVm>()
                .ForMember(f => f.Users, f => f.MapFrom(s => s.Users))
                .ForMember(f => f.LastMessage, f => f.MapFrom(s => s.Messages.OrderBy(f => f.CreatedOn).Select(w => w.Msg).FirstOrDefault()));
    }

    public class ChatUserVm : IMapFrom<ChatUser>
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Image { get; set; }
        public bool? Moderator { get; set; }
        public string SelfColor { get; set; }
        public string OthersColor { get; set; }

        public void Mapping(Profile profile) =>
            profile.CreateMap<ChatUser, ChatUserVm>()
                .ForMember(f => f.Moderator, f => f.MapFrom(s => s.IsModerator))
                .ForMember(f => f.Username, f => f.MapFrom(s => s.User.UserName))
                .ForMember(f => f.DisplayName, f => f.MapFrom(s => s.User.DisplayName))
                .ForMember(f => f.Image, f => f.MapFrom(s => s.User.Image));
    }
}
