using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Notifications.Queries.UserNotifications
{
    public class UserNotificationsHandler : IRequestHandler<UserNotificationsQuery, IEnumerable<NotificationVm>>
    {
        private readonly INotificationRepository _notification;
        private readonly ICurrentUserService _currentUser;

        public UserNotificationsHandler(INotificationRepository notification, ICurrentUserService currentUser)
        {
            _notification = notification ?? throw new ArgumentNullException(nameof(notification));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<IEnumerable<NotificationVm>> Handle(UserNotificationsQuery request, CancellationToken cancellationToken) =>
            await _notification.UserNotifications(_currentUser.User.Id, request.Skip, cancellationToken);
    }
}
