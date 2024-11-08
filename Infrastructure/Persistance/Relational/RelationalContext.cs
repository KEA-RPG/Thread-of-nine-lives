using Domain.Entities;
using Infrastructure.Persistance.Relational.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Infrastructure.Persistance.Relational
{
    public class RelationalContext : DbContext
    {   
        public RelationalContext(DbContextOptions options):base (options) { }
        public DbSet<Enemy> Enemies { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Deck> Decks { get; set; }
        public DbSet<DeckCard> DeckCards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new DeckCardConfiguration().Configure(modelBuilder.Entity<DeckCard>());
            new DeckConfiguration().Configure(modelBuilder.Entity<Deck>());
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Fight> Fights { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<GameAction> GameActions { get; set; }
    }
}
