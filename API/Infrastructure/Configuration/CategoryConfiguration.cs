using API.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using API.Domain.Constants;

namespace API.Infrastructure.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(ValidationConstants.CategoryNameMaxLength);

            builder.HasIndex(c => c.Name)
                .IsUnique();

            builder.Property(c => c.Description)
                .HasMaxLength(ValidationConstants.CategoryDescriptionMaxLength);

            builder.HasData(
                new Category { Id = 1, Name = "Technology", Description = "Latest technology news and innovations." },
                new Category { Id = 2, Name = "Business", Description = "Business strategies, startups, and entrepreneurship." },
                new Category { Id = 3, Name = "Finance", Description = "Personal finance, investing, and money management." },
                new Category { Id = 4, Name = "Health", Description = "Health tips, fitness, and wellness." },
                new Category { Id = 5, Name = "Lifestyle", Description = "Everyday living, habits, and inspiration." },
                new Category { Id = 6, Name = "Travel", Description = "Travel guides, destinations, and adventures." },
                new Category { Id = 7, Name = "Food & Recipes", Description = "Recipes, cooking tips, and food culture." },
                new Category { Id = 8, Name = "Education", Description = "Learning resources and educational content." },
                new Category { Id = 9, Name = "Science", Description = "Scientific discoveries and research." },
                new Category { Id = 10, Name = "Sports", Description = "Sports news, events, and analysis." },
                new Category { Id = 11, Name = "Entertainment", Description = "Movies, TV shows, music, and celebrities." },
                new Category { Id = 12, Name = "Fashion", Description = "Style, clothing, and fashion trends." },
                new Category { Id = 13, Name = "Photography", Description = "Photography tips and inspiration." },
                new Category { Id = 14, Name = "Gaming", Description = "Video games, reviews, and esports." },
                new Category { Id = 15, Name = "Books", Description = "Book reviews and reading recommendations." },
                new Category { Id = 16, Name = "Parenting", Description = "Parenting advice and family life." },
                new Category { Id = 17, Name = "Environment", Description = "Sustainability and environmental awareness." },
                new Category { Id = 18, Name = "Politics", Description = "Political news and discussions." },
                new Category { Id = 19, Name = "Culture", Description = "Arts, traditions, and society." },
                new Category { Id = 20, Name = "Opinion", Description = "Personal opinions and editorial articles." }
            );
        }
    }
}
