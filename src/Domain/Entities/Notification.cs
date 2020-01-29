using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class Notification : AuditableEntity
    {
        public long Id { get; set; }
        public string ForUserId { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public long? PostId { get; set; }
        public Post Post { get; set; }
        public NotificationType NotificationType { get; set; }
    }
}
