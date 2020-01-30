using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Domain.Entities;
using MediatR;
using System;
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

            var result = await _userFollow.Create(new UserFollow
            {
                FollowerId = user.Id,
                FollowingId = _currentUser.User.Id
            }, cancellationToken);

            if (!result.Succeeded)
                throw new BadRequestException("Could not create user follow because:\n" + string.Join('\n', result.Errors));

            return Unit.Value;
        }
    }
}
