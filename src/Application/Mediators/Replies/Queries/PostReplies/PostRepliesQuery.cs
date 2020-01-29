using System;
using System.Collections.Generic;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Replies.Queries.PostReplies
{
    public class PostRepliesQuery : IRequest<IEnumerable<PostVm>>
    {
        public long PostId { get; set; }
        public DateTime Skip { get; set; }
    }
}
