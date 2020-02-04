using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Multimedia.Queries.MultimediaPosts
{
    public class MultimediaPostsHandler : IRequestHandler<MultimediaPostsQuery, IEnumerable<PostVm>>
    {
        private readonly IPostRepository _post;
        private readonly ICurrentUserService _currentUser;
        private readonly ILikedPostRepository _likedPost;

        public MultimediaPostsHandler(IPostRepository post, ICurrentUserService currentUser, ILikedPostRepository likedPost)
        {
            _post = post ?? throw new ArgumentNullException(nameof(post));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _likedPost = likedPost ?? throw new ArgumentNullException(nameof(likedPost));
        }

        public async Task<IEnumerable<PostVm>> Handle(MultimediaPostsQuery request, CancellationToken cancellationToken)
        {
            var results = await _post.MultimediaPosts(request.Username, request.Skip, cancellationToken);

            var liked = await _likedPost.HasUserLikedPosts(results.Select(f => f.Id), _currentUser.User.Id, cancellationToken);

            return results.Select(f =>
            {
                f.IsLiked = liked.Contains(f.Id);
                return f;
            });
        }
    }
}
