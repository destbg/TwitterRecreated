using MediatR;

namespace Application.Follow.Command.FollowUser
{
    public class FollowUserCommand : IRequest
    {
        public string Username { get; set; }
    }
}
