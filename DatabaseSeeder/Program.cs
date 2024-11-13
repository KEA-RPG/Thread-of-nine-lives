using Domain.Entities;
using Infrastructure.Persistance;
using Infrastructure.Persistance.Relational;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;


namespace DataSeeder
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                PersistanceConfiguration.ConfigureServices(services, dbtype.DefaultConnection);
            });

            var host = builder.Build();

            SeedDatabase(host);
        }

        private static void SeedDatabase(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<RelationalContext>();

                // Ensure database is created
                context.Database.EnsureCreated();

                // Seed data
                if (!context.Users.Any())
                {
                    List<User> users = GenerateUsers();

                    //Encrypt the passwords
                    foreach (var user in users)
                    {
                        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
                    }

                    context.Users.AddRange(users);
                    context.SaveChanges();
                }


                if (!context.Cards.Any())
                {
                    var cards = GenerateCards();
                    context.Cards.AddRange(cards);
                    context.SaveChanges();
                }

                //Enemy
                if (!context.Enemies.Any())
                {
                    var enemies = GenerateEnemies();
                    context.Enemies.AddRange(enemies);
                    context.SaveChanges();
                }


                //Tag fat i det der allerede bliver oprettet fra Cards.json
                if (!context.Decks.Any())
                {
                    //List<Deck> decks = GenerateDecks();

                    Random random = new Random();

                    //Sætter maksimal mængde af kort der eksistere i databasen
                    var amountOfCards = context.Cards.Count();

                    //Henter alle kort der eksistere i databasen
                    var existingCards = context.Cards.ToList();

                    //Array til at holde Id'er på kort
                    int[] cardIdRange = new int[amountOfCards];

                    //Tilføjer alle id'er til arrayet
                    foreach (var cardId in existingCards)
                    {
                        cardIdRange = cardIdRange.Append(cardId.Id).ToArray();
                    }


                    

                    //Single Deck with a single card
                    var decks = new Deck
                    {
                        UserId = 133,
                        Name = "First Deck",
                        IsPublic = true
                    };

                    decks.DeckCards = new List<DeckCard>
                    {
                        new DeckCard
                        {
                            DeckId = decks.Id,
                            CardId = 1012
                        }
                    };



                    context.Decks.AddRange(decks);
                    context.SaveChanges();
                }


            }
        }

        private static List<Enemy> GenerateEnemies()
        {
            var enemies = Reader<List<Enemy>>("Enemies.json");
            return enemies;
        }

        private static List<Card> GenerateCards()
        {
            var cards = Reader<List<Card>>("Cards.json");
            return cards;
        }

        private static List<Deck> GenerateDecks()
        {
            var decks = Reader<List<Deck>>("Decks.json");
            return decks;
        }

        private static List<User> GenerateUsers()
        {
            var users = Reader<List<User>>("Users.json");
            return users;
        }

        private static T Reader<T>(string filePath)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            string jsonString = File.ReadAllText(filePath);
            var jsonDoc = JsonDocument.Parse(jsonString);
            return JsonSerializer.Deserialize<T>(jsonDoc, options);
        }
    }
}
