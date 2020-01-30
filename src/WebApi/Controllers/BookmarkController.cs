using System;
using System.Threading.Tasks;
using Application.Bookmarks.Command.CreateBookmark;
using Application.Bookmarks.Command.DeleteBookmark;
using Application.Bookmarks.Queries.UserBookmarks;
using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    public class BookmarkController : BaseController
    {
        private readonly IDateTime _date;

        public BookmarkController(IDateTime date)
        {
            _date = date ?? throw new ArgumentNullException(nameof(date));
        }

        [HttpGet("{skip?}")]
        public async Task<IActionResult> GetAllBookmarks(DateTime? skip) =>
            Ok(await Mediator.Send(new UserBookmarksQuery { Skip = skip ?? _date.MinDate }));

        [HttpPost]
        public async Task<IActionResult> CreateBookmark(CreateBookmarkCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookmark(long id)
        {
            await Mediator.Send(new DeleteBookmarkCommand { Id = id });
            return NoContent();
        }
    }
}
