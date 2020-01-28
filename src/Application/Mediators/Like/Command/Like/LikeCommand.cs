using MediatR;

namespace Application.Like.Command.Like
{
    public class LikeCommand : IRequest
    {
        public long PostId { get; set; }
    }
}
