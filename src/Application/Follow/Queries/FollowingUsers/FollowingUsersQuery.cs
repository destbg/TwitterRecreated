using MediatR;
using System.Collections.Generic;

namespace Application.Follow.Queries.FollowingUsers
{
    public class FollowingUsersQuery : IRequest<IEnumerable<string>>
    {
    }
}
