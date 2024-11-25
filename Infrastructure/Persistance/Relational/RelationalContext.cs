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
        public DbSet<AuditLog> AuditLogs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DeckConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new FightConfiguration());

            modelBuilder.Entity<Card>().ToTable(x => x.HasTrigger("trg_Audit_Cards_Insert"));
            modelBuilder.Entity<Card>().ToTable(x => x.HasTrigger("trg_Audit_Cards_Update"));
            modelBuilder.Entity<Card>().ToTable(x => x.HasTrigger("trg_Audit_Cards_Delete"));

            modelBuilder.Entity<Comment>().ToTable(x => x.HasTrigger("trg_Audit_Comments_Insert"));
            modelBuilder.Entity<Comment>().ToTable(x => x.HasTrigger("trg_Audit_Comments_Update"));
            modelBuilder.Entity<Comment>().ToTable(x => x.HasTrigger("trg_Audit_Comments_Delete"));

            modelBuilder.Entity<DeckCard>().ToTable(x => x.HasTrigger("trg_Audit_DeckCards_Insert"));
            modelBuilder.Entity<DeckCard>().ToTable(x => x.HasTrigger("trg_Audit_DeckCards_Update"));
            modelBuilder.Entity<DeckCard>().ToTable(x => x.HasTrigger("trg_Audit_DeckCards_Delete"));

            modelBuilder.Entity<Deck>().ToTable(x => x.HasTrigger("trg_Audit_Decks_Insert"));
            modelBuilder.Entity<Deck>().ToTable(x => x.HasTrigger("trg_Audit_Decks_Update"));
            modelBuilder.Entity<Deck>().ToTable(x => x.HasTrigger("trg_Audit_Decks_Delete"));

            modelBuilder.Entity<Enemy>().ToTable(x => x.HasTrigger("trg_Audit_Enemies_Insert"));
            modelBuilder.Entity<Enemy>().ToTable(x => x.HasTrigger("trg_Audit_Enemies_Update"));
            modelBuilder.Entity<Enemy>().ToTable(x => x.HasTrigger("trg_Audit_Enemies_Delete"));

            modelBuilder.Entity<Fight>().ToTable(x => x.HasTrigger("trg_Audit_Fights_Insert"));
            modelBuilder.Entity<Fight>().ToTable(x => x.HasTrigger("trg_Audit_Fights_Update"));
            modelBuilder.Entity<Fight>().ToTable(x => x.HasTrigger("trg_Audit_Fights_Delete"));

            modelBuilder.Entity<GameAction>().ToTable(x => x.HasTrigger("trg_Audit_GameActions_Insert"));
            modelBuilder.Entity<GameAction>().ToTable(x => x.HasTrigger("trg_Audit_GameActions_Update"));
            modelBuilder.Entity<GameAction>().ToTable(x => x.HasTrigger("trg_Audit_GameActions_Delete"));

            modelBuilder.Entity<User>().ToTable(x => x.HasTrigger("trg_Audit_Users_Insert"));
            modelBuilder.Entity<User>().ToTable(x => x.HasTrigger("trg_Audit_Users_Update"));
            modelBuilder.Entity<User>().ToTable(x => x.HasTrigger("trg_Audit_Users_Delete"));

        }
    }
}
