namespace API.Domain.Models
{
    public class Like
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }

        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
