using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Notifications.Command.CreateMultipleNotifications
{
    public class CreateMultipleNotificationsHandler : IRequestHandler<CreateMultipleNotificationsCommand>
    {
        private readonly INotificationRepository _notification;
        private readonly IUserFollowRepository _userFollow;
        private readonly ICurrentUserService _currentUser;

        public CreateMultipleNotificationsHandler(INotificationRepository notification, IUserFollowRepository userFollow, ICurrentUserService currentUser)
        {
            _notification = notification ?? throw new ArgumentNullException(nameof(notification));
            _userFollow = userFollow ?? throw new ArgumentNullException(nameof(userFollow));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<Unit> Handle(CreateMultipleNotificationsCommand request, CancellationToken cancellationToken)
        {
            var notifications = (await _userFollow.FollowingUsers(_currentUser.User.Id, cancellationToken))
                .Select(f => new Notification
                {
                    ForUserId = f,
                    NotificationType = request.NotificationType,
                    PostId = request.PostId,
                    UserId = _currentUser.User.Id
                });
            await _notification.CreateMany(notifications, cancellationToken);
            return Unit.Value;
        }
    }
}
