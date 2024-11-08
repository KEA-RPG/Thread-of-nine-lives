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


            // Configure relationship with Deck entity
            builder.HasMany(c => c.Comments)
                   .WithOne(d => d.Deck);
                   
        }
    }
}
