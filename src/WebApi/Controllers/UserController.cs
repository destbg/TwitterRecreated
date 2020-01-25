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
        public async Task<IActionResult> GetUserByName(string username)
        {
            var result = await Mediator.Send(new GetUserQuery { Username = username });
            return Ok(result);
        }

        [HttpGet("Post/{username}/{skip?}")]
        public async Task<IActionResult> GetUserPosts(string username, DateTime? skip)
        {
            var result = await Mediator.Send(new GetUserPostsQuery
            {
                Username = username,
                Skip = skip ?? _date.MinDate
            });
            return Ok(result);
        }
    }
}
