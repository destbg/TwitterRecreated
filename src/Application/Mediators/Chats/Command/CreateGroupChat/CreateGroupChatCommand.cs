using System.Collections.Generic;
using MediatR;

namespace Application.Chats.Command.CreateGroupChat
{
    public class CreateGroupChatCommand : IRequest
    {
        public IReadOnlyList<string> Users { get; set; }
    }
}
