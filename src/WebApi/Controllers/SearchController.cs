using System;
using System.Threading.Tasks;
using Application.Search.Queries.SearchImagePosts;
using Application.Search.Queries.SearchPosts;
using Application.Search.Queries.SearchTags;
using Application.Search.Queries.SearchUsers;
using Application.Search.Queries.SearchVideoPosts;
using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    public class SearchController : BaseController
    {
        private readonly IDateTime _date;

        public SearchController(IDateTime date)
        {
            _date = date ?? throw new ArgumentNullException(nameof(date));
        }

        [HttpGet("Post/{q}/{skip?}")]
        public async Task<IActionResult> GetSearchPosts(string q, DateTime? skip) =>
            Ok(await Mediator.Send(new SearchPostsQuery
            {
                Search = q,
                Skip = skip ?? _date.MinDate
            }));

        [HttpGet("Image/{q}/{skip?}")]
        public async Task<IActionResult> GetSearchImagePosts(string q, DateTime? skip) =>
            Ok(await Mediator.Send(new SearchImagePostsQuery
            {
                Search = q,
                Skip = skip ?? _date.MinDate
            }));

        [HttpGet("Video/{q}/{skip?}")]
        public async Task<IActionResult> GetSearchVideoPosts(string q, DateTime? skip) =>
            Ok(await Mediator.Send(new SearchVideoPostsQuery
            {
                Search = q,
                Skip = skip ?? _date.MinDate
            }));

        [HttpGet("User/{q}")]
        public async Task<IActionResult> GetSearchUsers(string q) =>
            Ok(await Mediator.Send(new SearchUsersQuery { Search = q }));

        [HttpGet("Tag/{q}")]
        public async Task<IActionResult> GetSearchTags(string q) =>
            Ok(await Mediator.Send(new SearchTagsQuery { Search = q }));
    }
}
