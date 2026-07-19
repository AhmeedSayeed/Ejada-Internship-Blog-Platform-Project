using Microsoft.AspNetCore.Identity;

namespace Blog_Project.Domain.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string? Bio { get; set; }
        public string? ProfileImageUrl { get; set; }
        public bool IsSuspended { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties

        public ICollection<Post> Posts { get; set; } = new List<Post>();

        public ICollection<Post> ReviewedPosts { get; set; } = new List<Post>();

        public ICollection<Follow> Following { get; set; } = new List<Follow>();

        public ICollection<Follow> Followers { get; set; } = new List<Follow>();

        public ICollection<Like> Likes { get; set; } = new List<Like>();

        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
