using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Notifications.Command.CreateNotification
{
    public class CreateNotificationHandler : IRequestHandler<CreateNotificationCommand>
    {
        private readonly INotificationRepository _notification;
        private readonly ICurrentUserService _currentUser;

        public CreateNotificationHandler(INotificationRepository notification, ICurrentUserService currentUser)
        {
            _notification = notification ?? throw new ArgumentNullException(nameof(notification));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<Unit> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            await _notification.Create(new Notification
            {
                ForUserId = request.UserId,
                NotificationType = request.NotificationType,
                PostId = request.PostId,
                UserId = _currentUser.User.Id
            }, cancellationToken);
            return Unit.Value;
        }
    }
}
