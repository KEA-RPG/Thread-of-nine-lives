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
