using System;
using System.Collections.Generic;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Search.Queries.SearchVideoPosts
{
    public class SearchVideoPostsQuery : IRequest<IEnumerable<PostVm>>
    {
        public string Search { get; set; }
        public DateTime Skip { get; set; }
    }
}
