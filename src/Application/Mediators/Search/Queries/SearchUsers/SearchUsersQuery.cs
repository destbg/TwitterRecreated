using System.Collections.Generic;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Search.Queries.SearchUsers
{
    public class SearchUsersQuery : IRequest<IEnumerable<UserFollowVm>>
    {
        public string Search { get; set; }
    }
}
