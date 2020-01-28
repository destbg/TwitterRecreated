using System;
using System.Collections.Generic;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Posts.Queries.MultimediaPosts
{
    public class MultimediaPostsQuery : IRequest<IEnumerable<PostVm>>
    {
        public string Username { get; set; }
        public DateTime Skip { get; set; }
    }
}
