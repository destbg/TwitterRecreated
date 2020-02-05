using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Application.Common.ViewModels;
using Common;
using MediatR;

namespace Application.Chats.Queries.UserChats
{
    public class UserChatsHandler : IRequestHandler<UserChatsQuery, IEnumerable<ChatVm>>
    {
        private readonly IChatRepository _chat;
        private readonly ICurrentUserService _currentUser;
        private readonly IConnectionMapping _connectionMapping;

        public UserChatsHandler(IChatRepository chat, ICurrentUserService currentUser, IConnectionMapping connectionMapping)
        {
            _chat = chat ?? throw new ArgumentNullException(nameof(chat));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _connectionMapping = connectionMapping ?? throw new ArgumentNullException(nameof(connectionMapping));
        }

        public async Task<IEnumerable<ChatVm>> Handle(UserChatsQuery request, CancellationToken cancellationToken)
        {
            var results = await _chat.UserChats(_currentUser.User.Id, cancellationToken);

            var usersOnline = _connectionMapping.UsersOnline(
                results.Where(f => !f.IsGroup)
                    .Select(f => f.Users
                        .FirstOrDefault(f => f.Username != _currentUser.User.UserName)?.Username)
            );

            return results.Select(f =>
            {
                if (!f.IsGroup)
                {
                    var user = f.Users.FirstOrDefault(f => f.Username != _currentUser.User.UserName);
                    if (usersOnline.Contains(user.Username))
                        user.IsOnline = true;
                }

                return f;
            });
        }
    }
}
