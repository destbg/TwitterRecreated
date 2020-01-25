namespace Domain.Entities
{
    public class PostImage
    {
        public long Id { get; set; }
        public string Image { get; set; }
        public long PostId { get; set; }
        public Post Post { get; set; }
    }
}
