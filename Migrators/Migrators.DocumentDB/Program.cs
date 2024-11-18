using Domain.DTOs;
using Infrastructure.Persistance;
using Infrastructure.Persistance.Relational;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistance.Document;

Console.WriteLine("Starting mongoDB migration");
var builder = Host.CreateDefaultBuilder(args)
.ConfigureServices(services =>
{
    PersistanceConfiguration.ConfigureServices(services, dbtype.DefaultConnection);
});


var host = builder.Build();
using (var scope = host.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    //get relational models
    var relationalContext = services.GetRequiredService<RelationalContext>();
    var mongoContext = services.GetRequiredService<DocumentContext>();
    Console.WriteLine("Setup of databases complete");
    var cards = relationalContext.Cards.ToList();
    var decks = relationalContext.Decks.Include(x => x.DeckCards).ThenInclude(x => x.Card).ToList();
    var enemies = relationalContext.Enemies.ToList();
    var users = relationalContext.Users.ToList();
    Console.WriteLine("Extrtracted data from relational database");

    //map to mongo models (dtos)
    var mongoCards = cards.Select(x => CardDTO.FromEntity(x));
    var mongoDecks = decks.Select(x => DeckDTO.FromEntity(x));
    var mongoEnemies = enemies.Select(x => EnemyDTO.FromEntity(x));
    var mongoUsers = users.Select(x => UserDTO.FromEntity(x));
    Console.WriteLine("Mapped data to mongoDB data");

    Console.WriteLine("Clearing out old database..");

    mongoContext.GetClient().DropDatabase("KeaRpg");
    Console.WriteLine("Old database dropped!");

    //insert into collections
    if (mongoCards?.Any() == true)
    {
        Console.WriteLine("Inserting card data into mongoDB...");
        mongoContext.Cards().InsertMany(mongoCards);
    }
    else
    {
        Console.WriteLine("No card data to insert into mongoDB.");
    }

    if (mongoDecks?.Any() == true)
    {
        Console.WriteLine("Inserting deck data into mongoDB...");
        mongoContext.Decks().InsertMany(mongoDecks);
    }
    else
    {
        Console.WriteLine("No deck data to insert into mongoDB.");
    }

    if (mongoEnemies?.Any() == true)
    {
        Console.WriteLine("Inserting enemy data into mongoDB...");
        mongoContext.Enemies().InsertMany(mongoEnemies);
    }
    else
    {
        Console.WriteLine("No enemy data to insert into mongoDB.");
    }

    if (mongoUsers?.Any() == true)
    {
        Console.WriteLine("Inserting user data into mongoDB...");
        mongoContext.Users().InsertMany(mongoUsers);
    }
    else
    {
        Console.WriteLine("No user data to insert into mongoDB.");
    }

    // Wait for all tasks to complete
    Console.WriteLine("All insert operations completed successfully.");

}