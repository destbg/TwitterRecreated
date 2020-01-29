using MediatR;

namespace Application.Reposts.Command.CreateRepostWithComment
{
    public class CreateRepostWithCommentCommand : IRequest
    {
        public string Content { get; set; }
        public long RepostId { get; set; }
    }
}
