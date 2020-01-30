using MediatR;

namespace Application.Chats.Command.IsUserInChat
{
    public class IsUserInChatCommand : IRequest
    {
        public long ChatId { get; set; }
    }
}
