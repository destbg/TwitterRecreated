using System;
using System.Threading.Tasks;
using Application.Replies.Command.CreatePostReply;
using Application.Replies.Queries.PostReplies;
using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class ReplyController : BaseController
    {
        private readonly IDateTime _date;

        public ReplyController(IDateTime date)
        {
            _date = date ?? throw new ArgumentNullException(nameof(date));
        }

        [HttpGet("{id}/{skip?}")]
        public async Task<IActionResult> PostReplies(long id, DateTime? skip) =>
            Ok(await Mediator.Send(new PostRepliesQuery { PostId = id, Skip = skip ?? _date.MinDate }));

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatePostReply(CreatePostReplyCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
