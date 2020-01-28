using System;
using System.Collections.Generic;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Bookmarks.Queries.UserBookmarks
{
    public class UserBookmarksQuery : IRequest<IEnumerable<PostVm>>
    {
        public DateTime Skip { get; set; }
    }
}
