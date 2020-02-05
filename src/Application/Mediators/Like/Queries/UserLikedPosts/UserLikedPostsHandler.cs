using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Like.Queries.UserLikedPosts
{
    public class UserLikedPostsHandler : IRequestHandler<UserLikedPostsQuery, IEnumerable<PostVm>>
    {
        private readonly ILikedPostRepository _likedPost;
        private readonly ICurrentUserService _currentUser;

        public UserLikedPostsHandler(ILikedPostRepository likedPost, ICurrentUserService currentUser)
        {
            _likedPost = likedPost ?? throw new ArgumentNullException(nameof(likedPost));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<IEnumerable<PostVm>> Handle(UserLikedPostsQuery request, CancellationToken cancellationToken) =>
            await _likedPost.UserPosts(request.Username, _currentUser.User.Id, request.Skip, cancellationToken);
    }
}
