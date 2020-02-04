using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Application.Common.ViewModels;
using AutoMapper;
using Common;
using Domain.Entities;
using MediatR;

namespace Application.Chats.Command.CreateGroupChat
{
    public class CreateGroupChatHandler : IRequestHandler<CreateGroupChatCommand, ChatVm>
    {
        private readonly IChatRepository _chat;
        private readonly ICurrentUserService _currentUser;
        private readonly IDateTime _date;
        private readonly IUserManager _userManager;
        private readonly IMapper _mapper;
        private readonly IMainHubService _mainHub;

        public CreateGroupChatHandler(IChatRepository chat, ICurrentUserService currentUser, IDateTime date, IUserManager userManager, IMapper mapper, IMainHubService mainHub)
        {
            _chat = chat ?? throw new ArgumentNullException(nameof(chat));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _date = date ?? throw new ArgumentNullException(nameof(date));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mainHub = mainHub ?? throw new ArgumentNullException(nameof(mainHub));
        }

        public async Task<ChatVm> Handle(CreateGroupChatCommand request, CancellationToken cancellationToken)
        {
            var users = await _userManager.ValidateUsersnames(request.Users);
            if (users.Count != request.Users.Count)
                throw new BadRequestException("Not all users in the list of users are valid");
            var chat = new Chat
            {
                Image = "assets/group/default.jpg",
                IsGroup = true,
                Name = string.Join(", ", request.Users).Substring(0, 50),
                CreatedOn = _date.Now
            };
            await _chat.Create(chat, cancellationToken);

            foreach (var user in users)
            {
                chat.ChatUsers.Add(new ChatUser
                {
                    ChatId = chat.Id,
                    IsModerator = false,
                    OthersColor = "8b0000",
                    SelfColor = "4b0082",
                    User = user
                });
            }
            chat.ChatUsers.Add(new ChatUser
            {
                ChatId = chat.Id,
                IsModerator = true,
                OthersColor = "8b0000",
                SelfColor = "4b0082",
                User = _currentUser.User
            });
            await _chat.Update(chat, cancellationToken);

            var mappedChat = _mapper.Map<ChatVm>(chat);
            await _mainHub.AddUsersToChat(chat.ChatUsers.Select(f => f.UserId).ToList(), mappedChat);

            return mappedChat;
        }
    }
}
