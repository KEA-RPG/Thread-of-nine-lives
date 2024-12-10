using Domain.DTOs;
using Infrastructure.Persistance;
using Infrastructure.Persistance.Relational;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistance.Document;
using Domain.Entities.Mongo;
using System.Diagnostics.Metrics;
using Infrastructure.Persistance.Graph;
using AutoMapper;
using Domain.Entities;
using GraphCard = Domain.Entities.Neo4J.Card;
using GraphComment = Domain.Entities.Neo4J.Comment;
using GraphDeck = Domain.Entities.Neo4J.Deck;
using GraphEnemy = Domain.Entities.Neo4J.Enemy;
using GraphFight = Domain.Entities.Neo4J.Fight;
using GraphGameAction = Domain.Entities.Neo4J.GameAction;
using GraphUser = Domain.Entities.Neo4J.User;

Console.WriteLine("Starting Neo4j migration");
var builder = Host.CreateDefaultBuilder(args)
.ConfigureServices(services =>
{
    PersistanceConfiguration.ConfigureServices(services, dbtype.DefaultConnection);
});
var config = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<Card, GraphCard>();
    cfg.CreateMap<Comment, GraphComment>();
    cfg.CreateMap<Deck, GraphDeck>();
    cfg.CreateMap<Enemy, GraphEnemy>();
    cfg.CreateMap<Fight, GraphFight>();
    cfg.CreateMap<GameAction, GraphGameAction>();
    cfg.CreateMap<User, GraphUser>();
});
var mapper = config.CreateMapper();

var host = builder.Build();
using (var scope = host.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    //get relational models
    var relationalContext = services.GetRequiredService<RelationalContext>();
    var graphContext = services.GetRequiredService<GraphContext>();
    Console.WriteLine("Setup of databases complete");

    var cards = relationalContext.Cards.ToList();
    var mappedCards = cards.Select(mapper.Map<GraphCard>);

    var decks = relationalContext.Decks.AsNoTracking().Include(x => x.DeckCards).ToList();
    var mappedDecks = decks.Select(mapper.Map<GraphDeck>);
    foreach (var deck in mappedDecks)
    {
        deck.Comments = new List<GraphComment>();
    }
    var enemies = relationalContext.Enemies.AsNoTracking().ToList();
    var mappedEnemies = enemies.Select(mapper.Map<GraphEnemy>);

    var users = relationalContext.Users.AsNoTracking().ToList();
    var mappedUsers = users.Select(mapper.Map<GraphUser>);

    var fights = relationalContext.Fights.AsNoTracking().ToList();
    var mappedFights = fights.Select(mapper.Map<GraphFight>);

    var gameActions = relationalContext.GameActions.AsNoTracking().ToList();
    var mappedGameActions = gameActions.Select(mapper.Map<GraphGameAction>);

    var comments = relationalContext.Comments.AsNoTracking().ToList();
    var mappedComments = comments.Select(mapper.Map<GraphComment>);
    Console.WriteLine("Extracted data from relational database");

    //insert into collections
    Console.WriteLine("Inserting card data into graphDB...");
    await graphContext.InsertManyNodes(mappedCards);
    await graphContext.InsertManyNodes(mappedDecks);
    await graphContext.InsertManyNodes(mappedEnemies);
    await graphContext.InsertManyNodes(mappedUsers);
    await graphContext.InsertManyNodes(mappedFights);
    await graphContext.InsertManyNodes(mappedGameActions);
    await graphContext.InsertManyNodes(mappedComments);
    Console.WriteLine("All insert operations completed successfully.");
    Console.WriteLine("Mapping relations");
    foreach (var deck in decks)
    {
        foreach (var card in deck.DeckCards)
        {
            await graphContext.MapNodes<GraphDeck, GraphCard>(deck.Id, card.Id, "CONTAINS");
        }
        await graphContext.MapNodes<GraphUser, GraphDeck>(deck.UserId, deck.Id, "OWNS");
    }
    Console.WriteLine("Deck --> Card done");
    Console.WriteLine("User --> Comment done");
    Console.WriteLine("User --> Deck done");
    foreach (var comment in comments)
    {
        await graphContext.MapNodes<GraphComment, GraphDeck>(comment.Id, comment.DeckId, "IS_IN");
        await graphContext.MapNodes<GraphUser, GraphComment>(comment.UserId, comment.Id, "WROTE");
    }
    Console.WriteLine("Comments --> Deck done");
    foreach (var fight in fights)
    {
        await graphContext.MapNodes<GraphEnemy, GraphFight>(fight.EnemyId, fight.Id, "FIGHTS_IN");
        await graphContext.MapNodes<GraphUser, GraphFight>(fight.UserId, fight.Id, "FIGHTS_IN");
        foreach (var gameAction in gameActions)
        {
            await graphContext.MapNodes<GraphGameAction, GraphFight>(gameAction.Id, fight.Id, "PART_OF");
        }

    }
    Console.WriteLine("Enemy --> Fight done");
    Console.WriteLine("User --> Fight done");
    Console.WriteLine("GameAction --> Fight done");
    Console.WriteLine("Creating counters...");
    await graphContext.InitCounter<GraphCard>(cards.Max(x=>x.Id));
    await graphContext.InitCounter<GraphComment>(comments.Max(x => x.Id));
    await graphContext.InitCounter<GraphDeck>(decks.Max(x => x.Id));
    await graphContext.InitCounter<GraphEnemy>(enemies.Max(x => x.Id));
    await graphContext.InitCounter<GraphFight>(fights.Max(x => x.Id));
    await graphContext.InitCounter<GraphGameAction>(gameActions.Max(x => x.Id));
    await graphContext.InitCounter<GraphUser>(users.Max(x => x.Id));

    Console.WriteLine("Mapping done");

}