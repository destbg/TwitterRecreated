using System.Collections.Generic;
using MediatR;

namespace Application.Chats.Queries.UserChatIds
{
    public class UserChatCheckQuery : IRequest<IEnumerable<UserChatCheckResponse>>
    {
    }
}
