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

        public SearchUsersHandler(IUserManager userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<IEnumerable<UserFollowVm>> Handle(SearchUsersQuery request, CancellationToken cancellationToken) =>
            await _userManager.SeachUsers(request.Search);
    }
}
