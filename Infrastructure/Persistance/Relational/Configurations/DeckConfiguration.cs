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
            builder.Property(b => b.Comments).HasDefaultValue(null);


        }
    }
}
