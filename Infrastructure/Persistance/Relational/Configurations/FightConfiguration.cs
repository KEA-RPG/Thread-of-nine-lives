using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance.Relational.Configurations
{
    class FightConfiguration : IEntityTypeConfiguration<Fight>
    {
        public void Configure(EntityTypeBuilder<Fight> builder)
        {
            builder.HasOne(x => x.Enemy)
                .WithMany(x=> x.Fights) 
                .HasForeignKey(x => x.EnemyId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.GameActions)
                .WithOne(x=> x.Fight)
                .HasForeignKey(x => x.FightId);

            builder.HasOne(x => x.User)
                .WithMany(x=> x.Fights)
                .HasForeignKey(x => x.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
