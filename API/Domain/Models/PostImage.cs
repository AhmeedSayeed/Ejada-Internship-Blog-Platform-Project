namespace API.Domain.Models
{
    public class PostImage
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
