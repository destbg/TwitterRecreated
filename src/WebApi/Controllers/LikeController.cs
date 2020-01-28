using Application.Like.Command.Like;
using Application.Like.Queries.UserLikedPosts;
using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Authorize]
    public class LikeController : BaseController
    {
        private readonly IDateTime _date;

        public LikeController(IDateTime date)
        {
            _date = date ?? throw new ArgumentNullException(nameof(date));
        }

        [HttpPost]
        public async Task<IActionResult> LikePost(LikeCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpGet("{username}/{skip?}")]
        public async Task<IActionResult> UserLikedPosts(string username, DateTime? skip) =>
            Ok(await Mediator.Send(new UserLikedPostsQuery
            {
                Username = username,
                Skip = skip ?? _date.MinDate
            }));
    }
}
