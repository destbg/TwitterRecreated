using System;

namespace Domain.Entities
{
    public class Chat
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsGroup { get; set; }
    }
}
