using Blog_Project.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog_Project.Infrastructure.Configuration
{
    public class PostImageConfiguration : IEntityTypeConfiguration<PostImage>
    {
        public void Configure(EntityTypeBuilder<PostImage> builder) 
        {
            builder.HasKey(pi => pi.Id);

            builder.Property(pi => pi.ImageUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(pi => pi.UploadedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(pi => pi.Post)
                .WithMany(p => p.Images)
                .HasForeignKey(pi => pi.PostId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}
