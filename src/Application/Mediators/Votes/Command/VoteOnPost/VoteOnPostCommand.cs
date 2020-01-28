using MediatR;

namespace Application.Votes.Command.VoteOnPost
{
    public class VoteOnPostCommand : IRequest
    {
        public long OptionId { get; set; }
    }
}
