using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.ViewModels;
using Application.Follow.Queries.FollowingUsers;
using Common;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using WebApi.Hubs;

namespace WebApi.Services
{
    public class MainHubService : IMainHubService
    {
        private readonly IHubContext<MainHub> _hubContext;
        private readonly IMediator _mediator;
        private readonly IConnectionMapping _connectionMapping;

        public MainHubService(IHubContext<MainHub> hubContext, IMediator mediator, IConnectionMapping connectionMapping)
        {
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _connectionMapping = connectionMapping ?? throw new ArgumentNullException(nameof(connectionMapping));
        }

        public async Task SendPost(PostVm post) =>
            await _hubContext.Clients
                .Users(await _mediator.Send(new FollowingUsersQuery()))
                .SendAsync("newPost", post);

        public Task SendLikedPost(LikeVm like) =>
            _hubContext.Clients
                .Group("post" + like.PostId)
                .SendAsync("likedPost", like);

        public Task SendPollVote(PollVoteVm pollVote) =>
            _hubContext.Clients
                .Group("post" + pollVote.PostId)
                .SendAsync("votedOnPoll", pollVote);

        public Task SendDeletedPost(long postId) =>
            _hubContext.Clients
                .Group("post" + postId)
                .SendAsync("deletedPost", postId);

        public Task SendMessage(MessageVm message) =>
            _hubContext.Clients
                .Group("msg" + message.ChatId)
                .SendAsync("newMessage", message);

        public async Task AddNewUserToChat(string username, ChatVm chat)
        {
            var connections = _connectionMapping.GetConnections(username).ToArray();

            foreach (var connection in connections)
                await _hubContext.Groups.AddToGroupAsync(connection, "msg" + chat.Id);

            await _hubContext.Clients
                .Clients(connections)
                .SendAsync("newChat", chat);
        }

        public async Task AddUsersToNewChat(IEnumerable<string> usernames, ChatVm chat)
        {
            var connections = _connectionMapping.UsersConnections(usernames).ToArray();

            foreach (var connection in connections)
                await _hubContext.Groups.AddToGroupAsync(connection, "msg" + chat.Id);

            await _hubContext.Clients
                .Clients(connections)
                .SendAsync("newChat", chat);
        }
    }
}
