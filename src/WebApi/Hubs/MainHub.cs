using Application.Posts.Command.VerifyPosts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace WebApi.Hubs
{
    [Authorize]
    public class MainHub : Hub
    {
        private readonly IMediator _mediator;

        public MainHub(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        //public override Task OnConnectedAsync()
        //{
        //    Context.UserIdentifier
        //}

        [HubMethodName("followPosts")]
        public async Task OnFollowPosts(long[] postIds)
        {
            if (postIds == null || postIds.Length == 0)
                return;

            if (await _mediator.Send(new VerifyPostsCommand { PostIds = postIds }))
            {
                foreach (var id in postIds)
                    await Groups.AddToGroupAsync(Context.ConnectionId, id.ToString());
            }
        }

        [HubMethodName("unFollowPosts")]
        public async Task OnUnFollowPosts(long[] postIds)
        {
            if (postIds == null || postIds.Length == 0)
                return;

            foreach (var id in postIds)
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, id.ToString());
        }
    }
}
