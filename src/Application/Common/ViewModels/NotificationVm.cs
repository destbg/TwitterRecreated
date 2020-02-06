using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Common.ViewModels
{
    public class NotificationVm : IMapFrom<Notification>
    {
        public long Id { get; set; }
        public NotificationType Type { get; set; }
        public PostShortVm Post { get; set; }
        public UserShortVm User { get; set; }

        public void Mapping(Profile profile) =>
            profile.CreateMap<Notification, NotificationVm>()
                .ForMember(f => f.Type, f => f.MapFrom(s => s.NotificationType))
                .ForMember(f => f.User, f => f.MapFrom(s => s.User))
                .ForMember(f => f.Post, f => f.MapFrom(s => s.Post));
    }
}
