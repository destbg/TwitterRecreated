using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Posts.Command.VerifyPosts
{
    public class VerifyPostsHandler : IRequestHandler<VerifyPostsCommand, bool>
    {
        private readonly IRepository<Post> _post;

        public VerifyPostsHandler(IRepository<Post> post)
        {
            _post = post ?? throw new ArgumentNullException(nameof(post));
        }

        public async Task<bool> Handle(VerifyPostsCommand request, CancellationToken cancellationToken) =>
            await _post.GetAll().CountAsync(f => request.PostIds.Contains(f.Id), cancellationToken) == request.PostIds.Length;
    }
}
