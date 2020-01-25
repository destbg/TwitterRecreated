namespace Domain.Entities
{
    public class ChatUser
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public long ChatId { get; set; }
        public bool? IsModerator { get; set; }
        public string SelfColor { get; set; }
        public string OthersColor { get; set; }
    }
}
