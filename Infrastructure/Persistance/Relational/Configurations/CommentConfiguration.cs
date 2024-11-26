using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Relational.Configurations
{
    internal class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            // Set primary key
            builder.HasKey(c => c.Id);

            // Configure relationship with User entity
            builder.HasOne(c => c.User)
                   .WithMany(u => u.Comments)
                   .HasForeignKey(c => c.UserId)
                   .OnDelete(DeleteBehavior.Restrict);  
                                                        
            builder.HasOne(c => c.Deck)
                   .WithMany(d => d.Comments)
                   .HasForeignKey(c => c.DeckId)
                   .OnDelete(DeleteBehavior.Restrict);


            // Create index on DeckId with included columns
            builder.HasIndex(c => c.DeckId)
                   .HasDatabaseName("idx_deck_id")
                   .IncludeProperties(c => new { c.Text, c.CreatedAt, c.UserId });

        }
    }
}
