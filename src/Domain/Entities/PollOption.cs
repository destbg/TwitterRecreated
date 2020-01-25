using System.Collections.Generic;

namespace Domain.Entities
{
    public class PollOption
    {
        public PollOption()
        {
            Votes = new HashSet<PollVote>();
        }

        public long Id { get; set; }
        public string Option { get; set; }
        public long PostId { get; set; }
        public Post Post { get; set; }

        public ICollection<PollVote> Votes { get; set; }
    }
}
