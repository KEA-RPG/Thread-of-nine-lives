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

Console.WriteLine("Starting mongoDB migration");
var builder = Host.CreateDefaultBuilder(args)
.ConfigureServices(services =>
{
    PersistanceConfiguration.ConfigureServices(services, dbtype.DefaultConnection);
});
var config = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<Card, GraphCard>();
    cfg.CreateMap<Card, GraphComment>();
    cfg.CreateMap<Card, GraphDeck>();
    cfg.CreateMap<Card, GraphEnemy>();
    cfg.CreateMap<Card, GraphFight>();
    cfg.CreateMap<Card, GraphGameAction>();
    cfg.CreateMap<Card, GraphUser>();
    cfg.CreateMap<Card, GraphCard>();
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
    var test = relationalContext.Cards.ToList();
    var cards = relationalContext.Cards.ToList().Select(mapper.Map<GraphCard>);
    var decks = relationalContext.Decks.ToList().Select(mapper.Map<GraphDeck>);
    var enemies = relationalContext.Enemies.ToList().Select(mapper.Map<GraphEnemy>);
    var users = relationalContext.Users.ToList().Select(mapper.Map<GraphUser>);
    var fights = relationalContext.Fights.ToList().Select(mapper.Map<GraphFight>);
    Console.WriteLine("Extracted data from relational database");

    //insert into collections
    Console.WriteLine("Inserting card data into graphDB...");
    await graphContext.InsertMany(cards, "Card");

    Console.WriteLine("All insert operations completed successfully.");

}