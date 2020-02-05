using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Application.Common.ViewModels;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Messages.Command.MessagesRead
{
    public class MessagesReadHandler : IRequestHandler<MessagesReadCommand, UserShortVm>
    {
        private readonly IChatRepository _chat;
        private readonly IMessageReadRepository _messageRead;
        private readonly IMessageRepository _messages;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;

        public MessagesReadHandler(IChatRepository chat, IMessageReadRepository messageRead, IMessageRepository messages, ICurrentUserService currentUser, IMapper mapper)
        {
            _chat = chat ?? throw new ArgumentNullException(nameof(chat));
            _messageRead = messageRead ?? throw new ArgumentNullException(nameof(messageRead));
            _messages = messages ?? throw new ArgumentNullException(nameof(messages));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<UserShortVm> Handle(MessagesReadCommand request, CancellationToken cancellationToken)
        {
            _ = await _chat.FindByUserAndChat(_currentUser.User.Id, request.ChatId, cancellationToken)
                ?? throw new BadRequestException("User is not in chat");

            var messageRead = await _messageRead.FindByUserAndChat(request.ChatId, _currentUser.User.Id, cancellationToken);
            if (messageRead.Count != 0)
                await _messageRead.DeleteMany(messageRead, cancellationToken);

            var message = await _messages.LastMessageInChat(request.ChatId, cancellationToken);
            await _messageRead.Create(new MessageRead
            {
                MessageId = message.Id,
                UserId = _currentUser.User.Id
            }, cancellationToken);
            return _mapper.Map<UserShortVm>(_currentUser.User);
        }
    }
}
