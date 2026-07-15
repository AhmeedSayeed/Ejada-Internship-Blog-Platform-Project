namespace Blog_Project.Domain.Models
{
    public class Follow
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int FollowerId { get; set; }
        public ApplicationUser Follower { get; set; }

        public int FollowingId { get; set; }
        public ApplicationUser FollowingUser { get; set; }
    }
}
