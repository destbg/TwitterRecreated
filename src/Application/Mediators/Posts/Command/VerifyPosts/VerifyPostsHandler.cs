using Application.Common.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Posts.Command.VerifyPosts
{
    public class VerifyPostsHandler : IRequestHandler<VerifyPostsCommand, bool>
    {
        private readonly IPostRepository _post;

        public VerifyPostsHandler(IPostRepository post)
        {
            _post = post ?? throw new ArgumentNullException(nameof(post));
        }

        public async Task<bool> Handle(VerifyPostsCommand request, CancellationToken cancellationToken) =>
            await _post.VerifyPosts(request.PostIds, cancellationToken) == request.PostIds.Length;
    }
}
