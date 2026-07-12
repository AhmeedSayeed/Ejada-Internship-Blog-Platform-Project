namespace Blog_Project.Domain.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        // Navigation properties

        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
