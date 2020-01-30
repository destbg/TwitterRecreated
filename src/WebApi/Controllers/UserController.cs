using Application.Users.Command.EditUserProfile;
using Application.Users.Queries.GetUser;
using Application.Users.Queries.GetUserPosts;
using Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class UserController : BaseController
    {
        private readonly IDateTime _date;

        public UserController(IDateTime date)
        {
            _date = date ?? throw new ArgumentNullException(nameof(date));
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetUserByName(string username) =>
            Ok(await Mediator.Send(new GetUserQuery { Username = username }));

        [HttpGet("Post/{username}/{skip?}")]
        public async Task<IActionResult> GetUserPosts(string username, DateTime? skip) =>
            Ok(await Mediator.Send(new GetUserPostsQuery
            {
                Username = username,
                Skip = skip ?? _date.MinDate
            }));

        [HttpPost("Profile")]
        public async  Task<IActionResult> EditUserProfile(EditUserProfileCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
