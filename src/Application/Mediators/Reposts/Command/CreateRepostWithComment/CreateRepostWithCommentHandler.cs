using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Reposts.Command.CreateRepostWithComment
{
    public class CreateRepostWithCommentHandler : IRequestHandler<CreateRepostWithCommentCommand>
    {
        private readonly IPostRepository _post;
        private readonly ICurrentUserService _currentUser;
        private readonly IUserManager _userManager;

        public CreateRepostWithCommentHandler(IPostRepository post, ICurrentUserService currentUser, IUserManager userManager)
        {
            _post = post ?? throw new ArgumentNullException(nameof(post));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<Unit> Handle(CreateRepostWithCommentCommand request, CancellationToken cancellationToken)
        {
            var post = await _post.GetById(request.RepostId, cancellationToken)
                ?? throw new NotFoundException("Repost Id", request.RepostId);

            if (post.UserId == _currentUser.UserId)
                throw new BadRequestException("You can't repost your own post");

            await _post.Create(new Post
            {
                Content = request.Content,
                Repost = post,
                User = await _userManager.GetUserById(_currentUser.UserId)
            }, cancellationToken);

            post.Reposts++;
            await _post.Update(post, cancellationToken);

            return Unit.Value;
        }
    }
}
