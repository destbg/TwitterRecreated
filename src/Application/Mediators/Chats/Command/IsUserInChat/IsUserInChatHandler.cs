using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using MediatR;

namespace Application.Chats.Command.IsUserInChat
{
    public class IsUserInChatHandler : IRequestHandler<IsUserInChatCommand>
    {
        private readonly IChatRepository _chat;
        private readonly ICurrentUserService _currentUser;

        public IsUserInChatHandler(IChatRepository chat, ICurrentUserService currentUser)
        {
            _chat = chat ?? throw new ArgumentNullException(nameof(chat));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<Unit> Handle(IsUserInChatCommand request, CancellationToken cancellationToken)
        {
            _ = await _chat.FindByUserAndChat(_currentUser.User.Id, request.ChatId, cancellationToken)
                ?? throw new NotFoundException("Chat Id", request.ChatId);
            return Unit.Value;
        }
    }
}
