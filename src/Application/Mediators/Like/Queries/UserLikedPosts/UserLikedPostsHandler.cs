using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Repositories;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Like.Queries.UserLikedPosts
{
    public class UserLikedPostsHandler : IRequestHandler<UserLikedPostsQuery, IEnumerable<PostVm>>
    {
        private readonly ILikedPostRepository _likedPost;

        public UserLikedPostsHandler(ILikedPostRepository likedPost)
        {
            _likedPost = likedPost ?? throw new ArgumentNullException(nameof(likedPost));
        }

        public async Task<IEnumerable<PostVm>> Handle(UserLikedPostsQuery request, CancellationToken cancellationToken) =>
            await _likedPost.UserPosts(request.Username, request.Skip, cancellationToken);
    }
}
