using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Search.Queries.SearchUsers
{
    public class SearchUsersHandler : IRequestHandler<SearchUsersQuery, IEnumerable<UserFollowVm>>
    {
        private readonly IUserManager _userManager;
        private readonly ICurrentUserService _currentUser;

        public SearchUsersHandler(IUserManager userManager, ICurrentUserService currentUser)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<IEnumerable<UserFollowVm>> Handle(SearchUsersQuery request, CancellationToken cancellationToken) =>
            await _userManager.SeachUsers(request.Search, _currentUser.User.Id);
    }
}
