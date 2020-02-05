using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Users.Queries.GetUserPosts
{
    public class GetUserPostsHandler : IRequestHandler<GetUserPostsQuery, IEnumerable<PostVm>>
    {
        private readonly IPostRepository _posts;
        private readonly ICurrentUserService _currentUser;

        public GetUserPostsHandler(IPostRepository posts, ICurrentUserService currentUser)
        {
            _posts = posts ?? throw new ArgumentNullException(nameof(posts));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<IEnumerable<PostVm>> Handle(GetUserPostsQuery request, CancellationToken cancellationToken) =>
            await _posts.UserPosts(request.Username, _currentUser.User?.Id, request.Skip, cancellationToken);
    }
}
