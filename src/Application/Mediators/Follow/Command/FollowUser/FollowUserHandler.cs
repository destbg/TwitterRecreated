using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Application.Notifications.Command.CreateNotification;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Follow.Command.FollowUser
{
    public class FollowUserHandler : IRequestHandler<FollowUserCommand>
    {
        private readonly IUserFollowRepository _userFollow;
        private readonly IUserManager _userManager;
        private readonly ICurrentUserService _currentUser;
        private readonly IMediator _mediator;

        public FollowUserHandler(IUserFollowRepository userFollow, IUserManager userManager, ICurrentUserService currentUser, IMediator mediator)
        {
            _userFollow = userFollow ?? throw new ArgumentNullException(nameof(userFollow));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<Unit> Handle(FollowUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserByUsername(request.Username)
                ?? throw new NotFoundException(nameof(request.Username), request.Username);

            if (user.Id == _currentUser.User.Id)
                throw new BadRequestException("You cannot follow yourself");

            var result = (
                await _userFollow.Find(f => f.FollowerId == _currentUser.User.Id
                    && f.FollowingId == user.Id, cancellationToken)
                ).FirstOrDefault();

            if (result == null)
            {
                _currentUser.User.Following++;
                user.Followers++;
                await _userFollow.Create(new UserFollow
                {
                    FollowerId = _currentUser.User.Id,
                    FollowingId = user.Id
                }, cancellationToken);
                if (_currentUser.User.Verified)
                {
                    await _mediator.Send(new CreateNotificationCommand
                    {
                        NotificationType = NotificationType.Follow,
                        UserId = user.Id
                    });
                }
            }
            else
            {
                await _userFollow.Delete(result, cancellationToken);
                _currentUser.User.Following--;
                await _userManager.UpdateUser(_currentUser.User);
                user.Followers--;
                await _userManager.UpdateUser(user);
                if (_currentUser.User.Verified)
                {
                    await _mediator.Send(new CreateNotificationCommand
                    {
                        NotificationType = NotificationType.UnFollow,
                        UserId = user.Id
                    });
                }
            }

            return Unit.Value;
        }
    }
}
