using Domain.Entities;
using Infrastructure.Persistance.Relational.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance.Relational
{
    public class RelationalContext : DbContext
    {   
        public RelationalContext(DbContextOptions options):base (options) { }
        public DbSet<Enemy> Enemies { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Deck> Decks { get; set; }
        public DbSet<DeckCard> DeckCards { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Fight> Fights { get; set; }
        public DbSet<GameAction> GameActions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DeckCardConfiguration());
            modelBuilder.ApplyConfiguration(new DeckConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new FightConfiguration());
        }
    }
}
