using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public DbSet<User> Users { get; set; }
        public DbSet<Fight> Fights { get; set; }
    }
}
