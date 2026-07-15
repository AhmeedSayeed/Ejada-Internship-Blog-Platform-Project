namespace Blog_Project.Domain.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int PostId { get; set; }
        public Post Post { get; set; }

        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
