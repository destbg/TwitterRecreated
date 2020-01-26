using Application.Common.Interfaces;
using Application.Common.ViewModels;
using Application.Follow.Queries.FollowingUsers;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
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

        public async Task SendLikedPost(LikeVm like) =>
            await _hubContext.Clients
                .Group(like.PostId.ToString())
                .SendAsync("likedPost", like);

        public async Task SendPollVote(PollVoteVm pollVote) =>
            await _hubContext.Clients
                .Group(pollVote.PostId.ToString())
                .SendAsync("votedOnPoll", pollVote);

        public async Task SendDeletedPost(long id) =>
            await _hubContext.Clients
                .Group(id.ToString())
                .SendAsync("deletedPost", id);
    }
}
