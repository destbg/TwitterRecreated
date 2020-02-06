using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Application.Common.ViewModels;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Chats.Command.CreateChat
{
    public class CreateChatHandler : IRequestHandler<CreateChatCommand, long?>
    {
        private readonly IChatRepository _chat;
        private readonly ICurrentUserService _currentUser;
        private readonly IUserManager _userManager;
        private readonly IMapper _mapper;
        private readonly IMainHubService _mainHub;

        public CreateChatHandler(IChatRepository chat, ICurrentUserService currentUser, IUserManager userManager, IMapper mapper, IMainHubService mainHub)
        {
            _chat = chat ?? throw new ArgumentNullException(nameof(chat));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mainHub = mainHub ?? throw new ArgumentNullException(nameof(mainHub));
        }

        public async Task<long?> Handle(CreateChatCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserByUsername(request.Username)
                ?? throw new NotFoundException(nameof(request.Username), request.Username);
            var chat = await _chat.FindChatByUsers(new[] { _currentUser.User.Id, user.Id }, false, cancellationToken);
            if (chat != null)
                return chat.Id;

            chat = new Chat();
            await _chat.Create(chat, cancellationToken);

            chat.ChatUsers.Add(new ChatUser
            {
                UserId = _currentUser.User.Id,
                ChatId = chat.Id,
                OthersColor = "8b0000",
                SelfColor = "4b0082"
            });
            chat.ChatUsers.Add(new ChatUser
            {
                UserId = user.Id,
                ChatId = chat.Id,
                OthersColor = "8b0000",
                SelfColor = "4b0082"
            });

            await _chat.Update(chat, cancellationToken);

            foreach (var chatUser in chat.ChatUsers)
                chatUser.User = chatUser.UserId == _currentUser.User.Id ? _currentUser.User : user;

            var mappedChat = _mapper.Map<ChatVm>(chat);
            await _mainHub.AddUsersToNewChat(chat.ChatUsers.Select(f => f.User.UserName).ToList(), mappedChat);

            return null;
        }
    }
}
