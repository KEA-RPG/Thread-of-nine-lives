using Domain.DTOs;
using Infrastructure.Persistance;
using Infrastructure.Persistance.Relational;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistance.Document;

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

    var cards = relationalContext.Cards.ToList();
    var decks = relationalContext.Decks.Include(x => x.DeckCards).ThenInclude(x => x.Card).ToList();
    var enemies = relationalContext.Enemies.ToList();
    var users = relationalContext.Users.ToList();

    //map to mongo models (dtos)
    var mongoCards = cards.Select(x => CardDTO.FromEntity(x));
    var mongoDecks = decks.Select(x => DeckDTO.FromEntity(x));
    var mongoEnemies = enemies.Select(x => EnemyDTO.FromEntity(x));
    var mongoUsers = users.Select(x => UserDTO.FromEntity(x));

    //insert into collections
    var insertCardsTask = mongoContext.Cards().InsertManyAsync(mongoCards);
    var insertDecksTask = mongoContext.Decks().InsertManyAsync(mongoDecks);
    var insertEnemiesTask = mongoContext.Enemies().InsertManyAsync(mongoEnemies);
    var insertUsersTask = mongoContext.Users().InsertManyAsync(mongoUsers);

    // Wait for all tasks to complete
    Task.WhenAll(insertCardsTask, insertDecksTask, insertEnemiesTask, insertUsersTask).Wait();
    Console.WriteLine("All insert operations completed successfully.");

}