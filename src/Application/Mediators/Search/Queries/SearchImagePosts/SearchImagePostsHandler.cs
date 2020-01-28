using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Repositories;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Search.Queries.SearchImagePosts
{
    public class SearchImagePostsHandler : IRequestHandler<SearchImagePostsQuery, IEnumerable<PostVm>>
    {
        private readonly IPostRepository _post;

        public SearchImagePostsHandler(IPostRepository post)
        {
            _post = post ?? throw new ArgumentNullException(nameof(post));
        }

        public async Task<IEnumerable<PostVm>> Handle(SearchImagePostsQuery request, CancellationToken cancellationToken) =>
            await _post.SearchImagePosts(request.Search, request.Skip, cancellationToken);
    }
}
