using MediatR;

namespace Application.Reposts.Command.CreateRepost
{
    public class CreateRepostCommand : IRequest
    {
        public long Id { get; set; }
        public string Content { get; set; }
    }
}
