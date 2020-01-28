using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Application.Posts.Command.CreatePost
{
    public class CreatePostCommand : IRequest
    {
        public List<IFormFile> Files { get; set; }
        public string Content { get; set; }
        public Uri Gif { get; set; }
        public string[] Poll { get; set; }
        public DateTime? PollEnd { get; set; }
    }
}
