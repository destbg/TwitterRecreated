﻿using System;
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

namespace Application.Chats.Command.CreateChat
{
    public class CreateChatHandler : IRequestHandler<CreateChatCommand, object>
    {
        private readonly IChatRepository _chat;
        private readonly ICurrentUserService _currentUser;
        private readonly IUserManager _userManager;
        private readonly IDateTime _date;
        private readonly IMapper _mapper;
        private readonly IMainHubService _mainHub;

        public CreateChatHandler(IChatRepository chat, ICurrentUserService currentUser, IUserManager userManager, IDateTime date, IMapper mapper, IMainHubService mainHub)
        {
            _chat = chat ?? throw new ArgumentNullException(nameof(chat));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _date = date ?? throw new ArgumentNullException(nameof(date));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mainHub = mainHub ?? throw new ArgumentNullException(nameof(mainHub));
        }

        public async Task<object> Handle(CreateChatCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserByUsername(request.Username)
                ?? throw new NotFoundException(nameof(request.Username), request.Username);
            var chat = await _chat.FindChatByUsers(new[] { _currentUser.User.Id, user.Id }, false, cancellationToken);
            if (chat != null)
                return chat.Id;
            chat = new Chat
            {
                Image = "",
                Name = "",
                IsGroup = false,
                CreatedOn = _date.Now
            };
            await _chat.Create(chat, cancellationToken);

            chat.ChatUsers.Add(new ChatUser
            {
                User = _currentUser.User,
                ChatId = chat.Id,
                OthersColor = "8b0000",
                SelfColor = "4b0082"
            });
            chat.ChatUsers.Add(new ChatUser
            {
                User = user,
                ChatId = chat.Id,
                OthersColor = "8b0000",
                SelfColor = "4b0082"
            });

            await _chat.Update(chat, cancellationToken);

            var mappedChat = _mapper.Map<ChatVm>(chat);
            await _mainHub.AddUsersToChat(chat.ChatUsers.Select(f => f.UserId).ToList(), mappedChat);

            return mappedChat;
        }
    }
}
