using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Repositories;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Posts.Queries.MultimediaPosts
{
    public class MultimediaPostsHandler : IRequestHandler<MultimediaPostsQuery, IEnumerable<PostVm>>
    {
        private readonly IPostRepository _post;

        public MultimediaPostsHandler(IPostRepository post)
        {
            _post = post ?? throw new ArgumentNullException(nameof(post));
        }

        public async Task<IEnumerable<PostVm>> Handle(MultimediaPostsQuery request, CancellationToken cancellationToken) =>
            await _post.MultimediaPosts(request.Username, request.Skip, cancellationToken);
    }
}
