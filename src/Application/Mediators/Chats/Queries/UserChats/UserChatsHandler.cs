using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Chats.Queries.UserChats
{
    public class UserChatsHandler : IRequestHandler<UserChatsQuery, IEnumerable<ChatVm>>
    {
        private readonly IChatRepository _chat;
        private readonly ICurrentUserService _currentUser;

        public UserChatsHandler(IChatRepository chat, ICurrentUserService currentUser)
        {
            _chat = chat ?? throw new ArgumentNullException(nameof(chat));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<IEnumerable<ChatVm>> Handle(UserChatsQuery request, CancellationToken cancellationToken) =>
            await _chat.UserChats(_currentUser.UserId, cancellationToken);
    }
}
