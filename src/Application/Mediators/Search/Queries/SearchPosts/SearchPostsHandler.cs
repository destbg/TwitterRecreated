using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Repositories;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Search.Queries.SearchPosts
{
    public class SearchPostsHandler : IRequestHandler<SearchPostsQuery, IEnumerable<PostVm>>
    {
        private readonly IPostRepository _post;

        public SearchPostsHandler(IPostRepository post)
        {
            _post = post ?? throw new ArgumentNullException(nameof(post));
        }

        public async Task<IEnumerable<PostVm>> Handle(SearchPostsQuery request, CancellationToken cancellationToken) =>
            await _post.SearchPosts(request.Search, request.Skip, cancellationToken);
    }
}
