using Domain.Enitities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistance.Relational
{
    public class RelationalContext : DbContext
    {
        public RelationalContext(DbContextOptions options):base (options) { }
        public DbSet<Card> Cards { get; set; }
    }
}
