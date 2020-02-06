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

namespace Application.Messages.Command.CreateMessage
{
    public class CreateMessageHandler : IRequestHandler<CreateMessageCommand>
    {
        private readonly IMessageRepository _message;
        private readonly IChatRepository _chat;
        private readonly ICurrentUserService _currentUser;
        private readonly IMainHubService _mainHub;
        private readonly IMapper _mapper;

        public CreateMessageHandler(IMessageRepository message, IChatRepository chat, ICurrentUserService currentUser, IMainHubService mainHub, IMapper mapper)
        {
            _message = message ?? throw new ArgumentNullException(nameof(message));
            _chat = chat ?? throw new ArgumentNullException(nameof(chat));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _mainHub = mainHub ?? throw new ArgumentNullException(nameof(mainHub));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Unit> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
        {
            var chat = await _chat.GetById(request.ChatId, cancellationToken)
                ?? throw new NotFoundException("Chat Id", request.ChatId);

            var message = new Message
            {
                ChatId = chat.Id,
                Msg = request.Content,
                UserId = _currentUser.User.Id
            };

            await _message.Create(message, cancellationToken);

            message.Chat = chat;
            message.User = _currentUser.User;
            await _mainHub.SendMessage(_mapper.Map<MessageVm>(message));

            return Unit.Value;
        }
    }
}
