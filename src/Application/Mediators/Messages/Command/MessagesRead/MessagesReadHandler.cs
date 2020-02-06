using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Application.Common.ViewModels;
using AutoMapper;
using MediatR;

namespace Application.Messages.Command.MessagesRead
{
    public class MessagesReadHandler : IRequestHandler<MessagesReadCommand, MessageReadResponse>
    {
        private readonly IChatUserRepository _chatUser;
        private readonly IMessageRepository _messages;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;

        public MessagesReadHandler(IChatUserRepository chatUser, IMessageRepository messages, ICurrentUserService currentUser, IMapper mapper)
        {
            _chatUser = chatUser ?? throw new ArgumentNullException(nameof(chatUser));
            _messages = messages ?? throw new ArgumentNullException(nameof(messages));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<MessageReadResponse> Handle(MessagesReadCommand request, CancellationToken cancellationToken)
        {
            var chatUser = await _chatUser.FindByUserAndChat(_currentUser.User.Id, request.ChatId, cancellationToken)
                ?? throw new BadRequestException("User is not in chat");
            var message = await _messages.LastMessageInChat(request.ChatId, cancellationToken);

            chatUser.MessageReadId = message.Id;
            await _chatUser.Update(chatUser, cancellationToken);

            return new MessageReadResponse
            {
                User = _mapper.Map<UserShortVm>(_currentUser.User),
                MessageId = message.Id
            };
        }
    }
}
