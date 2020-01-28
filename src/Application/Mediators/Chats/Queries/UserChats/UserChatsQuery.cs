using System.Collections.Generic;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Chats.Queries.UserChats
{
    public class UserChatsQuery : IRequest<IEnumerable<ChatVm>>
    {
    }
}
