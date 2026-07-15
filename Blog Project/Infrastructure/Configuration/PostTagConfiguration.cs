using Blog_Project.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog_Project.Infrastructure.Configuration
{
    public class PostTagConfiguration : IEntityTypeConfiguration<PostTag>
    {
        public void Configure(EntityTypeBuilder<PostTag> builder)
        {
            builder.HasKey(pt => new { pt.PostId, pt.TagId });

            builder.HasOne(pt => pt.Post)
                   .WithMany(p => p.PostTags)
                   .HasForeignKey(pt => pt.PostId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();

            builder.HasOne(pt => pt.Tag)
                   .WithMany(t => t.Posts)
                   .HasForeignKey(pt => pt.TagId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();
        }
    }
}
