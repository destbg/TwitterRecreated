using System;

namespace Domain.Entities
{
    public class Bookmark
    {
        public long Id { get; set; }
        public long PostId { get; set; }
        public Post Post { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public DateTime AddedOn { get; set; }
    }
}
