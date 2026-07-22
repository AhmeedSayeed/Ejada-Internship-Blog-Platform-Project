using API.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Configuration
{
    public class FollowConfiguration : IEntityTypeConfiguration<Follow>
    {
        public void Configure(EntityTypeBuilder<Follow> builder)
        {
            builder.HasKey(f => f.Id);

            builder.HasIndex(f => new { f.FollowerId, f.FollowingId })
                .IsUnique();

            builder.Property(f => f.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()"); ;

            builder.HasOne(f => f.Follower)
                .WithMany(u => u.Following)
                .HasForeignKey(f => f.FollowerId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasOne(f => f.FollowingUser)
                .WithMany(u => u.Followers)
                .HasForeignKey(f => f.FollowingId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();


            builder.ToTable(t => t.HasCheckConstraint(
                "CK_Follow_NoSelfFollow", "[FollowerId] <> [FollowingId]"));
        }
    }
}
