using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Common;
using Domain.Entities;
using MediatR;

namespace Application.Messages.Command.CreateMessage
{
    public class CreateMessageHandler : IRequestHandler<CreateMessageCommand>
    {
        private readonly IMessageRepository _message;
        private readonly IChatRepository _chat;
        private readonly IDateTime _date;
        private readonly ICurrentUserService _currentUser;

        public CreateMessageHandler(IMessageRepository message, IChatRepository chat, IDateTime date, ICurrentUserService currentUser)
        {
            _message = message ?? throw new ArgumentNullException(nameof(message));
            _chat = chat ?? throw new ArgumentNullException(nameof(chat));
            _date = date ?? throw new ArgumentNullException(nameof(date));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<Unit> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
        {
            _ = _chat.GetById(request.ChatId, cancellationToken)
                ?? throw new NotFoundException("Chat Id", request.ChatId);
            await _message.Create(new Message
            {
                ChatId = request.ChatId,
                CreatedAt = _date.Now,
                Msg = request.Content,
                UserId = _currentUser.UserId
            }, cancellationToken);
            return Unit.Value;
        }
    }
}
