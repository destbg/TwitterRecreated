using System;
using System.Collections.Generic;
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

        public SearchImagePostsHandler(IPostRepository post, ICurrentUserService currentUser)
        {
            _post = post ?? throw new ArgumentNullException(nameof(post));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<IEnumerable<PostVm>> Handle(SearchImagePostsQuery request, CancellationToken cancellationToken) =>
            await _post.SearchImagePosts(request.Search, _currentUser.User.Id, request.Skip, cancellationToken);
    }
}
