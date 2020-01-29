using Domain.Common;

namespace Domain.Entities
{
    public class UserFollow : AuditableEntity
    {
        public long Id { get; set; }
        public string FollowerId { get; set; }
        public AppUser Follower { get; set; }
        public string FollowingId { get; set; }
        public AppUser Following { get; set; }
    }
}
