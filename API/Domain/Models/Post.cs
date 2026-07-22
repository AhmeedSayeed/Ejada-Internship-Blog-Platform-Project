using API.Domain.Enums;

namespace API.Domain.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public PostStatus Status { get; set; }
        public int ViewCount { get; set; }
        public double AvgRating { get; set; }
        public int RatingCount { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public DateTime? PublishedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }


        // Navigation properties

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public int AuthorId { get; set; }
        public ApplicationUser Author { get; set; }

        public int? ReviewedByAdminId { get; set; }
        public ApplicationUser? ReviewedByAdmin { get; set; }

        public ICollection<Like> Likes { get; set; } = new List<Like>();

        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();

        public ICollection<PostImage> Images { get; set; } = new List<PostImage>();
    }
}
