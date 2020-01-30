using System;
using System.Threading.Tasks;
using Application.Chats.Command.AddUserToChat;
using Application.Chats.Command.IsUserInChat;
using Application.Chats.Queries.UserChatIds;
using Application.Common.Interfaces;
using Application.Messages.Command.MessagesRead;
using Application.Posts.Command.VerifyPosts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WebApi.Hubs
{
    [Authorize]
    public class MainHub : Hub
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUser;
        private readonly IUserManager _userManager;

        public MainHub(IMediator mediator, ICurrentUserService currentUser, IUserManager userManager)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public override async Task OnConnectedAsync()
        {
            await _currentUser.Initialize(Context.User, default, _userManager);
            foreach (var chat in await _mediator.Send(new UserChatIdsQuery()))
                await Groups.AddToGroupAsync(Context.ConnectionId, "msg" + chat);
        }

        [HubMethodName("followPosts")]
        public async Task OnFollowPosts(long[] postIds)
        {
            if (postIds == null || postIds.Length == 0)
                return;

            if (await _mediator.Send(new VerifyPostsCommand { PostIds = postIds }))
            {
                foreach (var id in postIds)
                    await Groups.AddToGroupAsync(Context.ConnectionId, "post" + id);
            }
        }

        [HubMethodName("unFollowPosts")]
        public async Task OnUnFollowPosts(long[] postIds)
        {
            if (postIds == null || postIds.Length == 0)
                return;

            foreach (var id in postIds)
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, "post" + id);
        }

        [HubMethodName("messagesRead")]
        public async Task OnMessagesRead(MessagesReadCommand command) =>
            await Clients.Group("msg" + command.ChatId).SendAsync("messagesWereRead",
                new { User = await _mediator.Send(command), command.ChatId });

        [HubMethodName("startTyping")]
        public async Task OnStartTyping(IsUserInChatCommand command)
        {
            await _mediator.Send(command);
            await Clients.Group("msg" + command.ChatId).SendAsync("userStartedTyping",
                new { Username = _currentUser.User.UserName, command.ChatId });
        }

        [HubMethodName("addUserToChat")]
        public async Task OnAddUserToChat(AddUserToChatCommand command) =>
            await Clients.Group("msg" + command.ChatId).SendAsync("userAddedToChat",
                new { command.ChatId, User = await _mediator.Send(command) });
    }
}
