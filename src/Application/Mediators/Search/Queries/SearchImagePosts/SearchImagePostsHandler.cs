using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Search.Queries.SearchImagePosts
{
    public class SearchImagePostsHandler : IRequestHandler<SearchImagePostsQuery, IEnumerable<PostVm>>
    {
        private readonly IPostRepository _post;
        private readonly ICurrentUserService _currentUser;
        private readonly ILikedPostRepository _likedPost;

        public SearchImagePostsHandler(IPostRepository post, ICurrentUserService currentUser, ILikedPostRepository likedPost)
        {
            _post = post ?? throw new ArgumentNullException(nameof(post));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _likedPost = likedPost ?? throw new ArgumentNullException(nameof(likedPost));
        }

        public async Task<IEnumerable<PostVm>> Handle(SearchImagePostsQuery request, CancellationToken cancellationToken)
        {
            var results = await _post.SearchImagePosts(request.Search, request.Skip, cancellationToken);

            var liked = await _likedPost.HasUserLikedPosts(results.Select(f => f.Id), _currentUser.User.Id, cancellationToken);

            return results.Select(f =>
            {
                f.IsLiked = liked.Contains(f.Id);
                return f;
            });
        }
    }
}
