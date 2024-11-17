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
        private static readonly ThreadLocal<Random> threadRandom = new(() => new Random()); //Bruger kun en tråd til at generere random tal

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
                //User
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

                //Card
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

                //Deck
                if (!context.Decks.Any())
                {

                    // Fetch all cards and users from the database
                    var existingCards = context.Cards.ToList();
                    var existingUsers = context.Users.ToList();

                    for (int i = 0; i < 50; i++)
                    {
                        var chosenCards = new List<Card>();

                        // Select 5 random cards for this deck, ensuring no duplicates
                        var selectedCardIds = new HashSet<int>();
                        while (chosenCards.Count < 5)
                        {
                            var chosenCardId = GetRandomNumber(0, existingCards.Count);
                            var card = existingCards[chosenCardId];

                            if (selectedCardIds.Add(card.Id)) // Add only if not already in the set
                            {
                                chosenCards.Add(card);
                            }
                        }

                        // Pick a random user
                        var chosenUserId = existingUsers[GetRandomNumber(0, existingUsers.Count)].Id;

                        // Create a new deck
                        var decks = new Deck
                        {
                            UserId = chosenUserId,
                            Name = GenerateDeckName(),
                            IsPublic = DeckIsPublic(chosenUserId),
                            DeckCards = new List<DeckCard>()
                        };

                        // Add unique cards to the deck
                        foreach (var card in chosenCards)
                        {
                            decks.DeckCards.Add(new DeckCard
                            {
                                DeckId = decks.Id, // DeckId will be assigned after SaveChanges
                                CardId = card.Id
                            });
                        }

                        context.Decks.Add(decks);
                    }

                    context.SaveChanges(); // Save all changes at once

                }
            }
        }


        private static string GenerateDeckName()
        {
            string[] firstPart = { "Super", "Mega", "Fire", "Water", "Test", "Meta", "Fun", "Tutorial" };
            string[] secondPart = { "Deck", "Combo", "Build", "List", "Collection", "Pile", "Stack", "Pack" };

            return $"{firstPart[GetRandomNumber(0, firstPart.Length)]} {secondPart[GetRandomNumber(0, secondPart.Length)]}";
        }

        private static bool DeckIsPublic(int num)
        {
            //Sætter public baseret på UserID
            bool IsPublic = num % 2 == 0;

            return IsPublic;
        }

        private static int GetRandomNumber(int min, int max)
        {
            return threadRandom.Value.Next(min, max);
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
