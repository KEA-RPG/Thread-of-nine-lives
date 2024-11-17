using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistance.Relational;
using System.Reflection;
using Infrastructure.Persistance.Document;


namespace Infrastructure.Persistance
{
    public static class PersistanceConfiguration
    {
        public static void ConfigureServices(IServiceCollection services, dbtype dbtype)
        {
            string dbString = dbtype.ToString();

            var binpath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var configuration = new ConfigurationBuilder()
                .SetBasePath(binpath)
                .AddJsonFile("dbsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString(dbString);

            services.AddDbContext<RelationalContext>(options =>
            {
                options.UseSqlServer(connectionString,
                b => b.MigrationsAssembly("Infrastructure"));
            });

            services.AddSingleton<DocumentContext>();
        }
    }

    public enum dbtype
    {
        DefaultConnection,
        Relational,
        MongoDB,
        Neo4j
    }
}
