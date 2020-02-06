using Domain.Enums;
using MediatR;

namespace Application.Notifications.Command.CreateMultipleNotifications
{
    public class CreateMultipleNotificationsCommand : IRequest
    {
        public NotificationType NotificationType { get; set; }
        public long? PostId { get; set; }
    }
}
