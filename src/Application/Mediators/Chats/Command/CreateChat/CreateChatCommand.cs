using MediatR;

namespace Application.Chats.Command.CreateChat
{
    public class CreateChatCommand : IRequest<object>
    {
        public string Username { get; set; }
    }
}
