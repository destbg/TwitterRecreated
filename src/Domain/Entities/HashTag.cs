namespace Domain.Entities
{
    public class HashTag
    {
        public long Id { get; set; }
        public string Tag { get; set; }
        public string Country { get; set; }
        public int Posts { get; set; }
    }
}
