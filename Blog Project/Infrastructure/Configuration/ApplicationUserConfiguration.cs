using Blog_Project.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Blog_Project.Domain.Constants;

namespace Blog_Project.Infrastructure.Configuration
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(u => u.Bio)
                .HasMaxLength(ValidationConstants.BioMaxLength);

            builder.Property(u => u.IsSuspended)
                .HasDefaultValue(false);

            builder.Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
