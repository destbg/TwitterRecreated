using System.Threading.Tasks;
using Application.Follow.Command.FollowUser;
using Application.Follow.Queries.FollowingFollowers;
using Application.Follow.Queries.FollowingUsers;
using Application.Follow.Queries.Suggestions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    public class FollowController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> FollowingUsers(FollowingUsersQuery command) =>
            Ok(await Mediator.Send(command));

        [HttpPost]
        public async Task<IActionResult> FollowUser(FollowUserCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpGet("FollowingFollowers")]
        public async Task<IActionResult> FollowingFollowers(FollowingFollowersQuery command) =>
            Ok(await Mediator.Send(command));

        [HttpGet("Suggestions")]
        public async Task<IActionResult> FollowersFollowers() =>
            Ok(await Mediator.Send(new SuggestionsQuery()));
    }
}
