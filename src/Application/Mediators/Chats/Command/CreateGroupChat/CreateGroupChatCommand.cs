using System.Collections.Generic;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Chats.Command.CreateGroupChat
{
    public class CreateGroupChatCommand : IRequest<ChatVm>
    {
        public IReadOnlyList<string> Users { get; set; }
    }
}
