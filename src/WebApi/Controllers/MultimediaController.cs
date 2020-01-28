using System;
using System.Threading.Tasks;
using Application.Posts.Queries.MultimediaPosts;
using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    public class MultimediaController : BaseController
    {
        private readonly IDateTime _date;

        public MultimediaController(IDateTime date)
        {
            _date = date ?? throw new ArgumentNullException(nameof(date));
        }

        [HttpGet("{username}/{skip?}")]
        public async Task<IActionResult> UserMultimediaPosts(string username, DateTime? skip) =>
            Ok(await Mediator.Send(new MultimediaPostsQuery
            {
                Username = username,
                Skip = skip ?? _date.MinDate
            }));
    }
}
