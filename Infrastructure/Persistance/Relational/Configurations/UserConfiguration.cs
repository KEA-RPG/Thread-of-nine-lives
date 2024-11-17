using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Relational.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Set properties
            builder.Property(u => u.Username)
                   .HasMaxLength(255)
                   .IsRequired();

            builder.Property(u => u.PasswordHash)
                   .HasMaxLength(512)
                   .IsRequired();

            builder.Property(u => u.Role)
                   .HasMaxLength(255)
                   .IsRequired();

            // Create index on Username with included columns
            builder.HasIndex(u => u.Username)
                   .HasDatabaseName("idx_username")
                   .IncludeProperties(u => new { u.PasswordHash, u.Role });
        }
    }
}
