using MediatR;

namespace Application.Messages.Command.CreateMessage
{
    public class CreateMessageCommand : IRequest
    {
        public string Content { get; set; }
        public long ChatId { get; set; }
    }
}
