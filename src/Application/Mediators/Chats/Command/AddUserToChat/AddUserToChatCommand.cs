using Application.Common.ViewModels;
using MediatR;

namespace Application.Chats.Command.AddUserToChat
{
    public class AddUserToChatCommand : IRequest<UserShortVm>
    {
        public string Username { get; set; }
        public long ChatId { get; set; }
    }
}
