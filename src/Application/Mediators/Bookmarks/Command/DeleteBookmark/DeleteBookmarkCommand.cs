using MediatR;

namespace Application.Bookmarks.Command.DeleteBookmark
{
    public class DeleteBookmarkCommand : IRequest
    {
        public long Id { get; set; }
    }
}
