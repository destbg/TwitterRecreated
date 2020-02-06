using Domain.Enums;
using MediatR;

namespace Application.Notifications.Command.CreateNotification
{
    public class CreateNotificationCommand : IRequest
    {
        public NotificationType NotificationType { get; set; }
        public string UserId { get; set; }
        public long? PostId { get; set; }
    }
}
