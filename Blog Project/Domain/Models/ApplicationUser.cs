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
    }
}
