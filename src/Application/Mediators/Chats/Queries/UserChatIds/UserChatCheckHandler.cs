using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using MediatR;

namespace Application.Chats.Queries.UserChatIds
{
    public class UserChatCheckHandler : IRequestHandler<UserChatCheckQuery, IEnumerable<UserChatCheckResponse>>
    {
        private readonly IChatRepository _chat;
        private readonly ICurrentUserService _currentUser;

        public UserChatCheckHandler(IChatRepository chat, ICurrentUserService currentUser)
        {
            _chat = chat ?? throw new ArgumentNullException(nameof(chat));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<IEnumerable<UserChatCheckResponse>> Handle(UserChatCheckQuery request, CancellationToken cancellationToken) =>
            await _chat.AllUserChats(_currentUser.User.Id, cancellationToken);
    }
}
