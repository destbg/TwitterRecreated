using MediatR;

namespace Application.Posts.Command.VerifyPosts
{
    public class VerifyPostsCommand : IRequest<bool>
    {
        public long[] PostIds { get; set; }
    }
}
