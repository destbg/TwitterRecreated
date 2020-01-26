using Application.Common.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;

namespace Application.Posts.Queries.FollowersPosts
{
    public class FollowersPostsQuery : IRequest<IEnumerable<PostVm>>
    {
        public DateTime Skip { get; set; }
    }
}
