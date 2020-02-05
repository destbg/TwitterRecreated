using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Posts.Queries.FollowersPosts
{
    public class FollowersPostsHandler : IRequestHandler<FollowersPostsQuery, IEnumerable<PostVm>>
    {
        private readonly IPostRepository _post;
        private readonly IUserFollowRepository _userFollow;
        private readonly ICurrentUserService _currentUser;

        public FollowersPostsHandler(IPostRepository post, IUserFollowRepository userFollow, ICurrentUserService currentUser)
        {
            _post = post ?? throw new ArgumentNullException(nameof(post));
            _userFollow = userFollow ?? throw new ArgumentNullException(nameof(userFollow));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<IEnumerable<PostVm>> Handle(FollowersPostsQuery request, CancellationToken cancellationToken)
        {
            var followers = await _userFollow.FollowingUsers(_currentUser.User.Id, cancellationToken);
            followers.Add(_currentUser.User.Id);
            return await _post.FindPostsFromUsers(followers, _currentUser.User.Id, request.Skip, cancellationToken);
        }
    }
}
