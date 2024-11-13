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
                if (!context.Cards.Any())
                {
                    var cards = GenerateCards();
                    context.Cards.AddRange(cards); //TODO: Make sure it gets added to the correct table
                    context.SaveChanges();
                }

                //Enemy
                if (!context.Enemies.Any())
                {
                    var enemies = GenerateEnemies();
                    context.Enemies.AddRange(enemies);
                    context.SaveChanges();
                }

                if (!context.Decks.Any())
                {
                    List<Deck> decks = GenerateDecks();


                    //This is a tests to see how the json file should look like
                    foreach (var deck in decks)
                    {
                        deck.DeckCards = new List<DeckCard>
                            {
                                new DeckCard
                                {
                                    DeckId = deck.Id,
                                    CardId = 1,
                                    Deck = deck,
                                    Card = new Card{
                                        Id = 1,
                                        Name = "First Battle",
                                        Description = "It's this little fellas first battle! He can aid in hitting, but have not yet learned to defend himself",
                                        Attack = 3,
                                        Defence =  0,
                                        Cost = 0,
                                        ImagePath = "1.jpg"
                                    }
                                }
                            };
                    }

                    context.Decks.AddRange(decks);
                    context.SaveChanges();
                }

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
