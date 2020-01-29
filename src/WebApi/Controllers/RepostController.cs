using System.Threading.Tasks;
using Application.Reposts.Command.CreateRepost;
using Application.Reposts.Command.CreateRepostWithComment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    public class RepostController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> CreateRepost(CreateRepostCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateRepostWithComment(CreateRepostWithCommentCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
