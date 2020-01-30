using System;
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

        public CreateChatHandler(IChatRepository chat, ICurrentUserService currentUser, IUserManager userManager, IDateTime date, IMapper mapper)
        {
            _chat = chat ?? throw new ArgumentNullException(nameof(chat));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _date = date ?? throw new ArgumentNullException(nameof(date));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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
                Image = "default.jpg",
                Name = "",
                IsGroup = false,
                CreatedOn = _date.Now
            };
            await _chat.Create(chat, cancellationToken);

            chat.Users.Add(new ChatUser
            {
                UserId = _currentUser.User.Id,
                ChatId = chat.Id,
                OthersColor = "#8b0000",
                SelfColor = "#4b0082"
            });
            chat.Users.Add(new ChatUser
            {
                UserId = user.Id,
                ChatId = chat.Id,
                OthersColor = "#8b0000",
                SelfColor = "#4b0082"
            });

            await _chat.Update(chat, cancellationToken);
            return _mapper.Map<ChatVm>(chat);
        }
    }
}
