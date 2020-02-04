using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Domain.Entities;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Follow.Command.FollowUser
{
    public class FollowUserHandler : IRequestHandler<FollowUserCommand>
    {
        private readonly IUserFollowRepository _userFollow;
        private readonly IUserManager _userManager;
        private readonly ICurrentUserService _currentUser;

        public FollowUserHandler(IUserFollowRepository userFollow, IUserManager userManager, ICurrentUserService currentUser)
        {
            _userFollow = userFollow ?? throw new ArgumentNullException(nameof(userFollow));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
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
                )
                .FirstOrDefault();

            if (result == null)
            {
                _currentUser.User.Following++;
                user.Followers++;
                await _userFollow.Create(new UserFollow
                {
                    Follower = _currentUser.User,
                    Following = user
                }, cancellationToken);
            }
            else
            {
                await _userFollow.Delete(result, cancellationToken);
                _currentUser.User.Following--;
                await _userManager.UpdateUser(_currentUser.User);
                user.Followers--;
                await _userManager.UpdateUser(user);
            }

            return Unit.Value;
        }
    }
}
