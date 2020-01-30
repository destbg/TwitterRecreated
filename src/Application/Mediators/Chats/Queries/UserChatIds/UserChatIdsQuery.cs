using System.Collections.Generic;
using MediatR;

namespace Application.Chats.Queries.UserChatIds
{
    public class UserChatIdsQuery : IRequest<IEnumerable<long>>
    {
    }
}
