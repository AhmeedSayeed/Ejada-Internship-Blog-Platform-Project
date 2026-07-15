using Blog_Project.Domain.Enums;
using Blog_Project.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog_Project.Infrastructure.Configuration
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Content)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(c => c.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasDefaultValue(CommentStatus.PendingApproval)
                .IsRequired();

            builder.Property(c => c.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasOne(c => c.ParentComment)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.ToTable(t => t.HasCheckConstraint("CK_Comment_Status", 
                "Status IN ('PendingApproval', 'Approved', 'Rejected', 'Deleted', 'Flagged')"));

            builder.ToTable(t => t.HasCheckConstraint("CK_Comment_Parent_Cycle",
                "ParentCommentId IS NULL OR ParentCommentId != Id"));
        }
    }
}
