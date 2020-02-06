using MediatR;

namespace Application.Chats.Command.CreateChat
{
    public class CreateChatCommand : IRequest<long?>
    {
        public string Username { get; set; }
    }
}
