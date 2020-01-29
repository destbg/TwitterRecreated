using Application.Posts.Command.CreatePost;
using Application.Posts.Command.DeletePost;
using Application.Posts.Queries.FollowersPosts;
using Application.Posts.Queries.GetPostById;
using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class PostController : BaseController
    {
        private readonly IDateTime _dateTime;

        public PostController(IDateTime dateTime)
        {
            _dateTime = dateTime ?? throw new ArgumentNullException(nameof(dateTime));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id) =>
            Ok(await Mediator.Send(new GetPostByIdQuery { Id = id }));

        [Authorize]
        [HttpGet("All/{skip?}")]
        public async Task<IActionResult> FollowersPosts(DateTime? skip) =>
            Ok(await Mediator.Send(new FollowersPostsQuery { Skip = skip ?? _dateTime.MinDate }));

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody]CreatePostCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(long id)
        {
            await Mediator.Send(new DeletePostCommand { PostId = id });
            return NoContent();
        }
    }
}
