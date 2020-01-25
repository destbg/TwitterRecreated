using System;

namespace Domain.Entities
{
    public class Message
    {
        public long Id { get; set; }
        public string Msg { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public long ChatId { get; set; }
        public Chat Chat { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? EditedAt { get; set; }
    }
}
