using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance.Relational.Configurations
{
    class DeckConfiguration
    {
        public void Configure(EntityTypeBuilder<Deck> builder)
        {
            builder.Property(b => b.IsPublic).HasDefaultValue(false);


            // Create index on UserId with included columns
            builder.HasIndex(d => d.UserId)
                   .HasDatabaseName("idx_user_id")
                   .IncludeProperties(d => new { d.Name, d.IsPublic });

            // Create index on IsPublic with included columns
            builder.HasIndex(d => d.IsPublic)
                   .HasDatabaseName("idx_is_public")
                   .IncludeProperties(d => new { d.UserId, d.Name });


        }
    }
}
