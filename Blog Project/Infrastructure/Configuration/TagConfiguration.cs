using Blog_Project.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog_Project.Infrastructure.Configuration
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(t => t.Name)
                .IsUnique();

            builder.HasData(
                new Tag { Id = 1, Name = "Trending" },
                new Tag { Id = 2, Name = "Featured" },
                new Tag { Id = 3, Name = "Popular" },
                new Tag { Id = 4, Name = "Beginner" },
                new Tag { Id = 5, Name = "Advanced" },
                new Tag { Id = 6, Name = "Guide" },
                new Tag { Id = 7, Name = "Tutorial" },
                new Tag { Id = 8, Name = "Tips" },
                new Tag { Id = 9, Name = "How-To" },
                new Tag { Id = 10, Name = "Review" },
                new Tag { Id = 11, Name = "Opinion" },
                new Tag { Id = 12, Name = "Analysis" },
                new Tag { Id = 13, Name = "News" },
                new Tag { Id = 14, Name = "Interview" },
                new Tag { Id = 15, Name = "Case Study" },
                new Tag { Id = 16, Name = "Lifestyle" },
                new Tag { Id = 17, Name = "Health" },
                new Tag { Id = 18, Name = "Fitness" },
                new Tag { Id = 19, Name = "Travel" },
                new Tag { Id = 20, Name = "Food" },
                new Tag { Id = 21, Name = "Recipes" },
                new Tag { Id = 22, Name = "Business" },
                new Tag { Id = 23, Name = "Startup" },
                new Tag { Id = 24, Name = "Marketing" },
                new Tag { Id = 25, Name = "Finance" },
                new Tag { Id = 26, Name = "Investing" },
                new Tag { Id = 27, Name = "Technology" },
                new Tag { Id = 28, Name = "AI" },
                new Tag { Id = 29, Name = "Programming" },
                new Tag { Id = 30, Name = "Science" },
                new Tag { Id = 31, Name = "Sports" },
                new Tag { Id = 32, Name = "Gaming" },
                new Tag { Id = 33, Name = "Movies" },
                new Tag { Id = 34, Name = "Music" },
                new Tag { Id = 35, Name = "Books" },
                new Tag { Id = 36, Name = "Photography" },
                new Tag { Id = 37, Name = "Fashion" },
                new Tag { Id = 38, Name = "Productivity" },
                new Tag { Id = 39, Name = "Inspiration" },
                new Tag { Id = 40, Name = "Daily Life" }
            );
        }
    }
}
