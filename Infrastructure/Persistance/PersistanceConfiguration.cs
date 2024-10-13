using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistance.Relational;
using System.Reflection;


namespace Infrastructure.Persistance
{
    public static class PersistanceConfiguration
    {
        public static void ConfigureServices(IServiceCollection services, dbtype dbtype)
        {
            string dbString = dbtype.ToString();

            var test = Directory.GetCurrentDirectory();

            // This will get the current PROJECT bin directory (ie ../bin/)
            string projectDirectory = Directory.GetParent(test).FullName;

            var nytest = Path.Combine(projectDirectory, "Infrastructure");
            var configuration = new ConfigurationBuilder()
                .SetBasePath(nytest)
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString(dbString);

            //var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<RelationalContext>(options =>
                options.UseSqlServer(connectionString,
                b => b.MigrationsAssembly("Infrastructure")));




        }
    }


    public enum dbtype
    {
        DefaultConnection,
        MongoDB,
        GraphDB
    }
}
