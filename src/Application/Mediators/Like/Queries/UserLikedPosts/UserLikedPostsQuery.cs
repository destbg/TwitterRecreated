using System;
using System.Collections.Generic;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Like.Queries.UserLikedPosts
{
    public class UserLikedPostsQuery : IRequest<IEnumerable<PostVm>>
    {
        public string Username { get; set; }
        public DateTime Skip { get; set; }
    }
}
