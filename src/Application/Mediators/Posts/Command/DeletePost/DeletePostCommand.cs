using MediatR;

namespace Application.Posts.Command.DeletePost
{
    public class DeletePostCommand : IRequest
    {
        public long PostId { get; set; }
    }
}
