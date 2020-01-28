using Application.Common.ViewModels;
using MediatR;
using System.Collections.Generic;

namespace Application.Follow.Queries.FollowingFollowers
{
    public class FollowingFollowersQuery : IRequest<IEnumerable<UserShortVm>>
    {
    }
}
