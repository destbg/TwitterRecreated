using System;
using System.Collections.Generic;
using System.Linq;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.ViewModels
{
    public class MessageVm : IMapFrom<Message>
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public UserShortVm User { get; set; }
        public long ChatId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public IEnumerable<UserShortVm> Users { get; set; }

        public void Mapping(Profile profile) =>
            profile.CreateMap<Message, MessageVm>()
                .ForMember(f => f.User, f => f.MapFrom(s => s.User))
                .ForMember(f => f.Message, f => f.MapFrom(s => s.Msg))
                .ForMember(f => f.Users, f => f.MapFrom(s => s.MessagesRead.Select(a => a.User)));
    }
}
