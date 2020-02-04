using System;
using System.IO;
using System.Threading.Tasks;
using Application.Posts.Command.CreatePost;
using Application.Posts.Command.DeletePost;
using Application.Posts.Queries.FollowersPosts;
using Application.Posts.Queries.GetPostById;
using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        public async Task<IActionResult> CreatePost([FromForm]CreatePostCommand command)
        {
            if (command == null || command.Content == null)
            {
                using var stream = new StreamReader(Request.Body);
                command = JsonConvert.DeserializeObject<CreatePostCommand>(await stream.ReadToEndAsync());
            }
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
