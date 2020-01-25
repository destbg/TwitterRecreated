using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Post
    {
        public Post()
        {
            Poll = new HashSet<PollOption>();
            Images = new HashSet<PostImage>();
        }

        public long Id { get; set; }
        public string Content { get; set; }
        public DateTime? PollEnd { get; set; }
        public string Video { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public long? ReplyId { get; set; }
        public Post Reply { get; set; }
        public long? RepostId { get; set; }
        public Repost Repost { get; set; }
        public int Reposts { get; set; }
        public int Likes { get; set; }
        public int Comments { get; set; }
        public DateTime PostedOn { get; set; }

        public ICollection<PollOption> Poll { get; set; }
        public ICollection<PostImage> Images { get; set; }
    }
}
