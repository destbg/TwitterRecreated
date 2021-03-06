﻿using System;
using System.Collections.Generic;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Search.Queries.SearchImagePosts
{
    public class SearchImagePostsQuery : IRequest<IEnumerable<PostVm>>
    {
        public string Search { get; set; }
        public DateTime Skip { get; set; }
    }
}
