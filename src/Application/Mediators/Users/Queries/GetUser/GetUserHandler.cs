using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Users.Queries.GetUser
{
    public class GetUserHandler : IRequestHandler<GetUserQuery, UserVm>
    {
        private readonly IUserManager _userManager;
        private readonly ICurrentUserService _currentUser;

        public GetUserHandler(IUserManager userManager, ICurrentUserService currentUser)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<UserVm> Handle(GetUserQuery request, CancellationToken cancellationToken) =>
            await _userManager.GetUserViewModel(request.Username, _currentUser.User.Id);
    }
}
