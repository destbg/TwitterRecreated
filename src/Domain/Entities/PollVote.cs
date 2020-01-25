namespace Domain.Entities
{
    public class PollVote
    {
        public long Id { get; set; }
        public long OptionId { get; set; }
        public PollOption Option { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}
