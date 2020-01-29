using Domain.Common;

namespace Domain.Entities
{
    public class LikedPost : AuditableEntity
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public long PostId { get; set; }
        public Post Post { get; set; }
    }
}
