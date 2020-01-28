using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Repositories;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Search.Queries.SearchVideoPosts
{
    public class SearchVideoPostsHandler : IRequestHandler<SearchVideoPostsQuery, IEnumerable<PostVm>>
    {
        private readonly IPostRepository _post;

        public SearchVideoPostsHandler(IPostRepository post)
        {
            _post = post ?? throw new ArgumentNullException(nameof(post));
        }

        public async Task<IEnumerable<PostVm>> Handle(SearchVideoPostsQuery request, CancellationToken cancellationToken) =>
            await _post.SearchVideoPosts(request.Search, request.Skip, cancellationToken);
    }
}
