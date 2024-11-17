using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Relational.Configurations
{
    internal class FightConfiguration : IEntityTypeConfiguration<Fight>
    {
        public void Configure(EntityTypeBuilder<Fight> builder)
        {
            // Set primary key
            builder.HasKey(f => f.Id);

            // Configure relationship with User entity
            builder.HasOne(f => f.User)
                   .WithMany(u => u.Fights)
                   .HasForeignKey(f => f.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Configure relationship with Enemy entity
            builder.HasOne(f => f.Enemy)
                   .WithMany(e => e.Fights)
                   .HasForeignKey(f => f.EnemyId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
