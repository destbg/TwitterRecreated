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

namespace Application.Chats.Command.AddUserToChat
{
    public class AddUserToChatHandler : IRequestHandler<AddUserToChatCommand, UserShortVm>
    {
        private readonly IUserManager _userManager;
        private readonly IMapper _mapper;
        private readonly IChatRepository _chat;
        private readonly ICurrentUserService _currentUser;
        private readonly IMainHubService _mainHub;

        public AddUserToChatHandler(IUserManager userManager, IMapper mapper, IChatRepository chat, ICurrentUserService currentUser, IMainHubService mainHub)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _chat = chat ?? throw new ArgumentNullException(nameof(chat));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _mainHub = mainHub ?? throw new ArgumentNullException(nameof(mainHub));
        }

        public async Task<UserShortVm> Handle(AddUserToChatCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserByUsername(request.Username)
                ?? throw new NotFoundException("Username", request.Username);
            var chat = await _chat.FindByUserAndChat(_currentUser.User.Id, request.ChatId, cancellationToken)
                ?? throw new NotFoundException("Chat Id", request.ChatId);

            if (!chat.ChatUsers.Any(f => f.UserId == _currentUser.User.Id && f.IsModerator == true))
                throw new BadRequestException("You are not moderator in this chat");

            if (chat.ChatUsers.Any(f => f.UserId == user.Id))
                throw new BadRequestException("User is already in this chat");

            chat.ChatUsers.Add(new ChatUser
            {
                IsModerator = false,
                ChatId = chat.Id,
                UserId = user.Id,
                OthersColor = "8b0000",
                SelfColor = "4b0082"
            });

            await _chat.Update(chat, cancellationToken);
            await _mainHub.AddUserToChat(user.Id, _mapper.Map<ChatVm>(chat));

            return _mapper.Map<UserShortVm>(user);
        }
    }
}
