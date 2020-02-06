using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class Message : AuditableEntity
    {
        public Message()
        {
            ChatUsers = new HashSet<ChatUser>();
        }

        public long Id { get; set; }
        public string Msg { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public long ChatId { get; set; }
        public Chat Chat { get; set; }
        public DateTime? EditedOn { get; set; }

        public ICollection<ChatUser> ChatUsers { get; set; }
    }
}
