using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Persistance.Relational.Configurations
{
    public class DeckCardConfiguration : IEntityTypeConfiguration<DeckCard>
    {
        public void Configure(EntityTypeBuilder<DeckCard> builder)
        {
            builder.HasKey(dc => new { dc.DeckId, dc.CardId });
        }
    }
}
