namespace Blog_Project.Domain.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<PostTag> Posts { get; set; } = new List<PostTag>();
    }
}
