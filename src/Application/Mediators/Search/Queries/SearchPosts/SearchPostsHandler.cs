using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Search.Queries.SearchPosts
{
    public class SearchPostsHandler : IRequestHandler<SearchPostsQuery, IEnumerable<PostVm>>
    {
        private readonly IPostRepository _post;
        private readonly ICurrentUserService _currentUser;

        public SearchPostsHandler(IPostRepository post, ICurrentUserService currentUser)
        {
            _post = post ?? throw new ArgumentNullException(nameof(post));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<IEnumerable<PostVm>> Handle(SearchPostsQuery request, CancellationToken cancellationToken) =>
            await _post.SearchPosts(request.Search, _currentUser.User.Id, request.Skip, cancellationToken);
    }
}
