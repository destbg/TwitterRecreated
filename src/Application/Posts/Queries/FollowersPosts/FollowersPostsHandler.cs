using Application.Common.Repositories;
using Application.Common.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Posts.Queries.FollowersPosts
{
    public class FollowersPostsHandler : IRequestHandler<FollowersPostsQuery, IEnumerable<PostVm>>
    {
        private readonly IPostRepository _post;

        public FollowersPostsHandler(IPostRepository post)
        {
            _post = post ?? throw new ArgumentNullException(nameof(post));
        }

        public async Task<IEnumerable<PostVm>> Handle(FollowersPostsQuery request, CancellationToken cancellationToken) =>
            await _post.FindPostsFromUsers(request.Skip, cancellationToken);
    }
}
