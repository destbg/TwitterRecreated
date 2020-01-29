using System;
using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Replies.Command.CreatePostReply
{
    public class CreatePostReplyCommand : IRequest
    {
        public List<IFormFile> Files { get; set; }
        public string Content { get; set; }
        public Uri Gif { get; set; }
        public string[] Poll { get; set; }
        public DateTime? PollEnd { get; set; }
        public long ReplyId { get; set; }
    }
}
