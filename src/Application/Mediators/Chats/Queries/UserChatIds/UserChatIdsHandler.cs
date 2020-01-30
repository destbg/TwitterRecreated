using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using MediatR;

namespace Application.Chats.Queries.UserChatIds
{
    public class UserChatIdsHandler : IRequestHandler<UserChatIdsQuery, IEnumerable<long>>
    {
        private readonly IChatRepository _chat;
        private readonly ICurrentUserService _currentUser;

        public UserChatIdsHandler(IChatRepository chat, ICurrentUserService currentUser)
        {
            _chat = chat ?? throw new ArgumentNullException(nameof(chat));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<IEnumerable<long>> Handle(UserChatIdsQuery request, CancellationToken cancellationToken) =>
            await _chat.UserChatIds(_currentUser.User.Id, cancellationToken);
    }
}
