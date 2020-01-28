using MediatR;

namespace Application.Bookmarks.Command.CreateBookmark
{
    public class CreateBookmarkCommand : IRequest
    {
        public long PostId { get; set; }
    }
}
