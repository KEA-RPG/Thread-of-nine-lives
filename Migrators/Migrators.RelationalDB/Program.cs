using Infrastructure.Persistance.Relational;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

var binpath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

var configuration = new ConfigurationBuilder()
    .SetBasePath(binpath)
    .AddJsonFile("dbsettingsrelational.json")
    .Build();

var connectionString = configuration.GetConnectionString("DefaultConnection");

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddDbContext<RelationalContext>(options =>
        {
            options.UseSqlServer(connectionString,
                b => b.MigrationsAssembly("Infrastructure"));
        });
    });

using (var host = builder.Build())
{
    using (var scope = host.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<RelationalContext>();

        //Tager navnene på migrations for debugging
        var pendingMigrations = context.Database.GetPendingMigrations().ToList();

        if (pendingMigrations.Any())
        {
            var migrationsNames = string.Join(", ", pendingMigrations);

            Console.WriteLine($"Attempting to do database migration on {migrationsNames}");
        }
        else
        {
            Console.WriteLine("No pending migrations found.");
        }
        try
        {
            context.Database.Migrate();  // Apply pending migrations
            Console.WriteLine("Database migration completed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while migrating the database: {ex.Message}");
        }
    }
}
