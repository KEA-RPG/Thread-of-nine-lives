using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistance.Relational;
using System.Reflection;
using Infrastructure.Persistance.Document;
using Microsoft.Extensions.Options;


namespace Infrastructure.Persistance
{
    public static class PersistanceConfiguration
    {
        public static void ConfigureServices(IServiceCollection services, dbtype dbtype, string environmentName)
        {
            string dbString = dbtype.ToString();

            var binpath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var configuration = new ConfigurationBuilder()
                .SetBasePath(binpath)
                .AddJsonFile($"dbsettings.{environmentName}.json")
                .Build();

            var connectionString = configuration.GetConnectionString(dbString);
            Console.WriteLine(connectionString);
            services.AddDbContext<RelationalContext>(options =>
            {
                options.UseSqlServer(connectionString,
                b => b.MigrationsAssembly("Infrastructure"));
            });

            services.AddSingleton<DocumentContext>(options =>
            {
                var settings = configuration.GetSection("ConnectionStrings:MongoDB");
                var connectionString = settings.GetSection("Connectionstring").Value;
                var databaseName = settings.GetSection("DatabaseName").Value;

                return new DocumentContext(connectionString, databaseName);
            });
        }

        public static string GetConnectionString(dbtype dbtype)
        {
            var binpath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var configuration = new ConfigurationBuilder()
                .SetBasePath(binpath)
                .AddJsonFile("dbsettings.json")
                .Build();

            switch (dbtype)
            {
                case dbtype.Relational:

                    return configuration.GetConnectionString("DefaultConnection");

            }
            return "";
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
