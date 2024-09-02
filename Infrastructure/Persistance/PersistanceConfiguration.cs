using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistance.Relational;

namespace Infrastructure.Persistance
{
    public static class PersistanceConfiguration
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration, string dbtype)
        {
            var dbOptions = configuration.GetSection("connectionstrings").GetSection(dbtype);
            var connectionString = configuration.GetConnectionString("DefaultConnection");





        }
    }
}
