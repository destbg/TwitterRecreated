﻿using System;
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

namespace Application.Chats.Command.CreateGroupChat
{
    public class CreateGroupChatHandler : IRequestHandler<CreateGroupChatCommand>
    {
        private readonly IChatRepository _chat;
        private readonly ICurrentUserService _currentUser;
        private readonly IUserManager _userManager;
        private readonly IMapper _mapper;
        private readonly IMainHubService _mainHub;

        public CreateGroupChatHandler(IChatRepository chat, ICurrentUserService currentUser, IUserManager userManager, IMapper mapper, IMainHubService mainHub)
        {
            _chat = chat ?? throw new ArgumentNullException(nameof(chat));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mainHub = mainHub ?? throw new ArgumentNullException(nameof(mainHub));
        }

        public async Task<Unit> Handle(CreateGroupChatCommand request, CancellationToken cancellationToken)
        {
            var users = await _userManager.ValidateUsersnames(request.Users);
            if (users.Count != request.Users.Count)
                throw new BadRequestException("Not all users in the list of users are valid");

            var name = string.Join(", ", request.Users);
            var chat = new Chat
            {
                Image = "assets/group/default.jpg",
                IsGroup = true,
                Name = name.Substring(0, name.Length < 50 ? name.Length : 50),
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

            await _mainHub.AddUsersToNewChat(
                chat.ChatUsers.Select(f => f.User.UserName).ToArray(),
                _mapper.Map<ChatVm>(chat)
            );

            return Unit.Value;
        }
    }
}
