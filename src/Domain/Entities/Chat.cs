using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Chat
    {
        public Chat()
        {
            Users = new HashSet<ChatUser>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsGroup { get; set; }
        public ICollection<ChatUser> Users { get; set; }
    }
}
