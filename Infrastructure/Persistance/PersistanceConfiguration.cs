using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistance.Relational;
using System.Reflection;
using Infrastructure.Persistance.Document;
using Microsoft.Extensions.Options;
using Infrastructure.Persistance.Graph;
using System.Configuration;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver;
using System.Data.Entity;


namespace Infrastructure.Persistance
{
    public static class PersistanceConfiguration
    {
        public static void ConfigureServices(IServiceCollection services, dbtype dbtype, string environmentName)
        {
            string dbString = dbtype.ToString();
            var configuration = GetConfiguration(environmentName);
            var connectionString = configuration.GetConnectionString(dbString);

            services.AddDbContext<RelationalContext>(options =>
            {
                options.UseSqlServer(connectionString,
                b => b.MigrationsAssembly("Infrastructure"));
            });

            services.AddSingleton(options =>
            {
                return GetDocumentContext(environmentName);
            });

            services.AddSingleton(options =>
            {
                return GetGraphContext(environmentName);
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
        public static DocumentContext GetDocumentContext(string environmentName)
        {
            var configuration = GetConfiguration(environmentName);
            var settings = configuration.GetSection("ConnectionStrings:MongoDB");

            var connectionString = settings.GetSection("Connectionstring").Value;
            var databaseName = settings.GetSection("DatabaseName").Value;

            return new DocumentContext(connectionString, databaseName);

        }
        public static RelationalContext GetRelationalContext(string environmentName)
        {
            var configuration = GetConfiguration(environmentName);

            var connectionString = configuration.GetSection("ConnectionStrings:DefaultConnection").Value;

            var options = new DbContextOptionsBuilder<RelationalContext>()
                .UseSqlServer(connectionString).Options;

            return new RelationalContext(options);


        }
        public static GraphContext GetGraphContext(string environmentName)
        {
            var configuration = GetConfiguration(environmentName);

            var settings = configuration.GetSection("ConnectionStrings:Neo4j");

            var connectionString = settings.GetSection("Connectionstring").Value;
            var databaseName = settings.GetSection("DatabaseName").Value;
            var user = settings.GetSection("User").Value;
            var password = settings.GetSection("Password").Value;

            return new GraphContext(connectionString, user, password, databaseName);


        }

        public static IConfigurationRoot GetConfiguration(string environmentName)
        {
            var binpath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (string.IsNullOrWhiteSpace(environmentName))
            {
                environmentName = "Development";
            }
            Console.WriteLine("Going on " + environmentName);
            var configuration = new ConfigurationBuilder()
                .SetBasePath(binpath)
                .AddJsonFile($"dbsettings.{environmentName}.json")
                .Build();
            return configuration;


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
