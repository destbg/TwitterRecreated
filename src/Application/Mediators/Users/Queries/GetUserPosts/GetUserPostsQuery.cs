using Application.Common.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;

namespace Application.Users.Queries.GetUserPosts
{
    public class GetUserPostsQuery : IRequest<IEnumerable<PostVm>>
    {
        public string Username { get; set; }
        public DateTime Skip { get; set; }
    }
}
