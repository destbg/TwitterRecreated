using Application.Like.Command.Like;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Authorize]
    public class LikeController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> LikePost(LikeCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
