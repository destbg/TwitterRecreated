using Application.Common.Repositories;
using Application.Common.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries.GetUserPosts
{
    public class GetUserPostsHandler : IRequestHandler<GetUserPostsQuery, IEnumerable<PostVm>>
    {
        private readonly IPostRepository _posts;

        public GetUserPostsHandler(IPostRepository posts)
        {
            _posts = posts ?? throw new ArgumentNullException(nameof(posts));
        }

        public async Task<IEnumerable<PostVm>> Handle(GetUserPostsQuery request, CancellationToken cancellationToken) =>
            await _posts.UserPosts(request.Skip, request.Username, cancellationToken);
    }
}
