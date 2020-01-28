using System;
using System.Collections.Generic;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Notifications.Queries.UserNotifications
{
    public class UserNotificationsQuery : IRequest<IEnumerable<NotificationVm>>
    {
        public DateTime Skip { get; set; }
    }
}
