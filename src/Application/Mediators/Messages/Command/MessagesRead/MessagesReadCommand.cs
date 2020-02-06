using MediatR;

namespace Application.Messages.Command.MessagesRead
{
    public class MessagesReadCommand : IRequest<MessageReadResponse>
    {
        public long ChatId { get; set; }
    }
}
