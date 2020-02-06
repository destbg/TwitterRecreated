using System;
using System.Threading.Tasks;
using Application.Chats.Command.AddUserToChat;
using Application.Chats.Command.CallRequest;
using Application.Chats.Command.IsUserInChat;
using Application.Chats.Queries.UserChatIds;
using Application.Common.Interfaces;
using Application.Common.ViewModels;
using Application.Messages.Command.MessagesRead;
using Application.Posts.Command.VerifyPosts;
using AutoMapper;
using Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace WebApi.Hubs
{
    [Authorize]
    public class MainHub : Hub
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUser;
        private readonly IUserManager _userManager;
        private readonly IMapper _mapper;
        private readonly IConnectionMapping _connectionMapping;
        private readonly IMemoryCacheService _cacheService;
        private readonly ILogger<MainHub> _logger;

        public MainHub(IMediator mediator, ICurrentUserService currentUser, IUserManager userManager, IMapper mapper, IConnectionMapping connectionMapping, IMemoryCacheService cacheService, ILogger<MainHub> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _connectionMapping = connectionMapping ?? throw new ArgumentNullException(nameof(connectionMapping));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override async Task OnConnectedAsync()
        {
            await _currentUser.Initialize(Context.User, default, _userManager, _cacheService);

            foreach (var chat in await _mediator.Send(new UserChatCheckQuery()))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "msg" + chat.Id);
                if (!chat.IsGroup)
                    await Clients.User(chat.UserId).SendAsync("userNowOnline", chat.Username);
            }

            _connectionMapping.Add(Context.User.Identity.Name, Context.ConnectionId);

            _logger.LogInformation("User {0} Connected with connection id {1}", _currentUser.User.UserName, Context.ConnectionId);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await _currentUser.Initialize(Context.User, default, _userManager, _cacheService);

            foreach (var chat in await _mediator.Send(new UserChatCheckQuery()))
            {
                if (!chat.IsGroup)
                    await Clients.User(chat.UserId).SendAsync("userNowOffline", chat.Username);
            }

            _connectionMapping.Remove(Context.User.Identity.Name, Context.ConnectionId);
            _logger.LogInformation("User {0} Disconnected", Context.User.Identity.Name);
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
        public async Task OnMessagesRead(MessagesReadCommand command)
        {
            await _currentUser.Initialize(Context.User, default, _userManager, _cacheService);

            var messageRead = await _mediator.Send(command);
            if (messageRead == null)
                return;

            await Clients.Group("msg" + command.ChatId).SendAsync("messagesWereRead",
                new { messageRead.User, messageRead.MessageId, command.ChatId });
        }

        [HubMethodName("startTyping")]
        public async Task OnStartTyping(IsUserInChatCommand command)
        {
            await _currentUser.Initialize(Context.User, default, _userManager, _cacheService);
            await _mediator.Send(command);
            await Clients.Group("msg" + command.ChatId).SendAsync("userStartedTyping",
                new { Username = _currentUser.User.UserName, command.ChatId });
        }

        [HubMethodName("addUserToChat")]
        public async Task OnAddUserToChat(AddUserToChatCommand command)
        {
            await _currentUser.Initialize(Context.User, default, _userManager, _cacheService);
            await Clients.Group("msg" + command.ChatId).SendAsync("userAddedToChat",
                new { command.ChatId, User = await _mediator.Send(command) });
        }

        [HubMethodName("requestCall")]
        public async Task OnRequestCall(CallRequestCommand command)
        {
            await _currentUser.Initialize(Context.User, default, _userManager, _cacheService);
            var result = await _mediator.Send(command);
            if (result == null)
                await Clients.Client(Context.ConnectionId).SendAsync("nonOnline");

            await Clients.User(result.Id).SendAsync("callRequest", new
            {
                command.Id,
                User = _mapper.Map<UserShortVm>(_currentUser.User),
                command.Data
            });
        }

        [HubMethodName("respondToCall")]
        public async Task OnRespondToCall(RespondToCallVm chat)
        {
            var result = await _userManager.GetUserByUsername(chat.Username);
            if (result == null)
                return;

            if (chat.Accept)
                await Clients.User(result.Id).SendAsync("acceptRequest", chat.Data);
            else await Clients.User(result.Id).SendAsync("requestDenied");
        }
    }
}
