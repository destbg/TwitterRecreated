namespace Domain.Entities
{
    public class MessageRead
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public long MessageId { get; set; }
        public Message Message { get; set; }
    }
}
