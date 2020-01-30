using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class Chat : AuditableEntity
    {
        public Chat()
        {
            Users = new HashSet<ChatUser>();
            Messages = new HashSet<Message>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public bool IsGroup { get; set; }

        public ICollection<ChatUser> Users { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
