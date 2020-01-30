using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using MediatR;

namespace Application.Posts.Command.DeletePost
{
    public class DeletePostHandler : IRequestHandler<DeletePostCommand>
    {
        private readonly IPostRepository _post;
        private readonly ICurrentUserService _currentUser;
        private readonly IMainHubService _mainHub;

        public DeletePostHandler(IPostRepository post, ICurrentUserService currentUser, IMainHubService mainHub)
        {
            _post = post ?? throw new ArgumentNullException(nameof(post));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _mainHub = mainHub ?? throw new ArgumentNullException(nameof(mainHub));
        }

        public async Task<Unit> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _post.GetById(request.PostId, cancellationToken)
                ?? throw new NotFoundException("Post Id", request.PostId);

            if (post.UserId != _currentUser.User.Id)
                throw new BadRequestException("You are not the original poster of this post");

            await _post.Delete(post, cancellationToken);
            await _mainHub.SendDeletedPost(post.Id);

            return Unit.Value;
        }
    }
}
