using Domain.Common;

namespace Domain.Entities
{
    public class Repost : AuditableEntity
    {
        public long Id { get; set; }
        public long PostId { get; set; }
        public Post Post { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public string Content { get; set; }
    }
}
