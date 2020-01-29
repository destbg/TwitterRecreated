using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Repositories;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Replies.Queries.PostReplies
{
    public class PostRepliesHandler : IRequestHandler<PostRepliesQuery, IEnumerable<PostVm>>
    {
        private readonly IPostRepository _post;

        public PostRepliesHandler(IPostRepository post)
        {
            _post = post ?? throw new ArgumentNullException(nameof(post));
        }

        public async Task<IEnumerable<PostVm>> Handle(PostRepliesQuery request, CancellationToken cancellationToken) =>
            await _post.PostReplies(request.PostId, request.Skip, cancellationToken);
    }
}
