using System.Threading.Tasks;
using Application.Votes.Command.VoteOnPost;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    public class VoteController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> VoteOnPost(VoteOnPostCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
