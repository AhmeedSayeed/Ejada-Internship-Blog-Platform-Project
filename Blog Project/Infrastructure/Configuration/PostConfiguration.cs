using Blog_Project.Domain.Enums;
using Blog_Project.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Blog_Project.Domain.Constants;

namespace Blog_Project.Data.Configuration
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(ValidationConstants.PostTitleMaxLength);

            builder.Property(p => p.Content)
                .IsRequired();

            builder.Property(p => p.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasDefaultValue(PostStatus.Draft)
                .IsRequired();

            builder.ToTable(t => t.HasCheckConstraint(
            "CK_Post_Status",
            "[Status] IN ('Draft', 'PendingApproval', 'Approved', 'Rejected')"));

            builder.Property(p => p.ViewCount)
                .HasDefaultValue(0);

            builder.Property(p => p.AvgRating)
                .HasDefaultValue(0.0);

            builder.Property(p => p.RatingCount)
                .HasDefaultValue(0);

            builder.Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(p => p.LastUpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(p => p.Category)
                .WithMany(c => c.Posts)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                ;

            builder.HasOne(p => p.Author)
                .WithMany(a => a.Posts)
                .HasForeignKey(p => p.AuthorId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasOne(p => p.ReviewedByAdmin)
                .WithMany(a => a.ReviewedPosts)
                .HasForeignKey(p => p.ReviewedByAdminId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
