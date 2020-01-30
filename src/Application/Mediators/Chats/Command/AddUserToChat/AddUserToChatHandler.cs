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
using Microsoft.EntityFrameworkCore.Internal;

namespace Application.Chats.Command.AddUserToChat
{
    public class AddUserToChatHandler : IRequestHandler<AddUserToChatCommand, UserShortVm>
    {
        private readonly IUserManager _userManager;
        private readonly IMapper _mapper;
        private readonly IChatRepository _chat;
        private readonly IChatUserRepository _chatUser;
        private readonly ICurrentUserService _currentUser;

        public AddUserToChatHandler(IUserManager userManager, IMapper mapper, IChatRepository chat, IChatUserRepository chatUser, ICurrentUserService currentUser)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _chat = chat ?? throw new ArgumentNullException(nameof(chat));
            _chatUser = chatUser ?? throw new ArgumentNullException(nameof(chatUser));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<UserShortVm> Handle(AddUserToChatCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserByUsername(request.Username)
                ?? throw new NotFoundException("Username", request.Username);
            var chat = await _chat.FindByUserAndChat(_currentUser.User.Id, request.ChatId, cancellationToken)
                ?? throw new NotFoundException("Chat Id", request.ChatId);

            if (!chat.Users.Any(f => f.UserId == _currentUser.User.Id && f.IsModerator == true))
                throw new BadRequestException("You are not moderator in this chat");

            if (chat.Users.Any(f => f.UserId == user.Id))
                throw new BadRequestException("User is already in this chat");

            await _chatUser.Create(new ChatUser
            {
                IsModerator = false,
                ChatId = chat.Id,
                UserId = user.Id
            }, cancellationToken);

            return _mapper.Map<UserShortVm>(user);
        }
    }
}
