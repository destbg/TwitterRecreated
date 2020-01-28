using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using MediatR;

namespace Application.Chats.Command.ChangeColor
{
    public class ChangeColorHandler : IRequestHandler<ChangeColorCommand>
    {
        private readonly IChatUserRepository _chatUser;
        private readonly ICurrentUserService _currentUser;

        public ChangeColorHandler(IChatUserRepository chatUser, ICurrentUserService currentUser)
        {
            _chatUser = chatUser ?? throw new ArgumentNullException(nameof(chatUser));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<Unit> Handle(ChangeColorCommand request, CancellationToken cancellationToken)
        {
            var chatUser = await _chatUser.FindByUserAndChat(_currentUser.UserId, request.ChatId, cancellationToken)
                ?? throw new NotFoundException("Chat User", request.ChatId);
            chatUser.OthersColor = request.OthersColor;
            chatUser.SelfColor = request.SelfColor;
            await _chatUser.Update(chatUser, cancellationToken);
            return Unit.Value;
        }
    }
}
