using Domain.Common;

namespace Domain.Entities
{
    public class Bookmark : AuditableEntity
    {
        public long Id { get; set; }
        public long PostId { get; set; }
        public Post Post { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}
