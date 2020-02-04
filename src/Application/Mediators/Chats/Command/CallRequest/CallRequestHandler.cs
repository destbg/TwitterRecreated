using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Chats.Command.CallRequest
{
    public class CallRequestHandler : IRequestHandler<CallRequestCommand, AppUser>
    {
        private readonly IChatUserRepository _chatUser;
        private readonly ICurrentUserService _currentUser;

        public CallRequestHandler(IChatUserRepository chatUser, ICurrentUserService currentUser)
        {
            _chatUser = chatUser ?? throw new ArgumentNullException(nameof(chatUser));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<AppUser> Handle(CallRequestCommand request, CancellationToken cancellationToken) =>
            await _chatUser.FindUserInChat(request.Id, _currentUser.User.Id, cancellationToken);
    }
}
