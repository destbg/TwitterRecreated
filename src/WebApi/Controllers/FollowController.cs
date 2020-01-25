using Application.Follow.Command.FollowUser;
using Application.Follow.Queries.FollowingFollowers;
using Application.Follow.Queries.FollowingUsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Authorize]
    public class FollowController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> FollowUser(FollowUserCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> FollowingUsers(FollowingUsersQuery command) =>
            Ok(await Mediator.Send(command));

        [HttpGet("FollowingFollowers")]
        public async Task<IActionResult> FollowingFollowers(FollowingFollowersQuery command) =>
            Ok(await Mediator.Send(command));

        //[HttpGet("Suggestions")]
        //public async Task<IActionResult> FollowersFollowers(FollowersFollowersQuery command) =>
        //    Ok(await Mediator.Send(command));
    }
}
