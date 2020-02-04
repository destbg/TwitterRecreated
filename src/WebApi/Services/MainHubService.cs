using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.ViewModels;
using Application.Follow.Queries.FollowingUsers;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using WebApi.Hubs;

namespace WebApi.Services
{
    public class MainHubService : IMainHubService
    {
        private readonly IHubContext<MainHub> _hubContext;
        private readonly IMediator _mediator;

        public MainHubService(IHubContext<MainHub> hubContext, IMediator mediator)
        {
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
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

        public Task AddUserToChat(string userId, ChatVm chat) =>
            _hubContext.Clients
                .User(userId)
                .SendAsync("newChat", chat);

        public Task AddUsersToChat(IReadOnlyList<string> userIds, ChatVm chat) =>
            _hubContext.Clients
                .Users(userIds)
                .SendAsync("newChat", chat);
    }
}
