using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance.Relational
{
    public class RelationalContext : DbContext
    {
        public RelationalContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Card> Cards { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
