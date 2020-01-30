using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Follow.Queries.Suggestions
{
    public class SuggestionsHandler : IRequestHandler<SuggestionsQuery, IEnumerable<UserShortVm>>
    {
        private readonly IUserFollowRepository _userFollow;
        private readonly ICurrentUserService _currentUser;

        public SuggestionsHandler(IUserFollowRepository userFollow, ICurrentUserService currentUser)
        {
            _userFollow = userFollow ?? throw new ArgumentNullException(nameof(userFollow));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<IEnumerable<UserShortVm>> Handle(SuggestionsQuery request, CancellationToken cancellationToken) =>
            await _userFollow.Suggestions(_currentUser.User.Id, cancellationToken);
    }
}
