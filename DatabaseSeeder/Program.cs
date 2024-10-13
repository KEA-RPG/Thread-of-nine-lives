using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Domain.Entities;
using Infrastructure.Persistance.Relational;
using Backend.Services;
using Microsoft.Extensions.Configuration;
using Infrastructure.Persistance;


namespace DataSeeder
{

    class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            SeedDatabase(host);
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {

            var builder = Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                PersistanceConfiguration.ConfigureServices(services, dbtype.DefaultConnection);
            });


            return builder;
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
            }
        }

        private static List<Enemy> GenerateEnemies()
        {
            var enemies = new List<Enemy>();

            enemies.Add(new Enemy
            {
                Name = "Rat",
                Health = "10",
                ImagePath = "rat.jpg"
            });

            enemies.Add(new Enemy
            {
                Name = "Goblin",
                Health = "15",
                ImagePath = "goblin.jpg"
            });

            enemies.Add(new Enemy
            {
                Name = "Orc",
                Health = "20",
                ImagePath = "orc.jpg"
            });

            enemies.Add(new Enemy
            {
                Name = "Troll",
                Health = "25",
                ImagePath = "troll.jpg"
            });

            enemies.Add(new Enemy
            {
                Name = "Dragon",
                Health = "30",
                ImagePath = "dragon.jpg"
            });

            enemies.Add(new Enemy
            {
                Name = "Wraith",
                Health = "18",
                ImagePath = "wraith.jpg"
            });

            enemies.Add(new Enemy
            {
                Name = "Minotaur",
                Health = "28",
                ImagePath = "minotaur.jpg"
            });

            enemies.Add(new Enemy
            {
                Name = "Harpy",
                Health = "22",
                ImagePath = "harpy.jpg"
            });

            enemies.Add(new Enemy
            {
                Name = "Skeleton",
                Health = "15",
                ImagePath = "skeleton.jpg"
            });

            enemies.Add(new Enemy
            {
                Name = "Vampire",
                Health = "35",
                ImagePath = "vampire.jpg"
            });

            enemies.Add(new Enemy
            {
                Name = "Bjørggle",
                Health = "10000000",
                ImagePath = "sigmaChad.jpg"
            });

            return enemies;
        }

        private static List<Card> GenerateCards()
        {
            var cards = new List<Card>();

            // Generate cards here
            cards.Add(new Card
            {
                Name = "First battle!",
                Description = "It's this little fellas first battle! He can aid in hitting, but have not yet learned to defend himself",
                Attack = 3,
                Defense = 0,
                Cost = 0,
                ImagePath = "1.jpg"
            });

            cards.Add(new Card
            {
                Name = "Guidance",
                Description = "With the guidence of his mentor, he is now a little bit more sturdy",
                Attack = 3,
                Defense = 2,
                Cost = 1,
                ImagePath = "2.jpg"

            });

            cards.Add(new Card
            {
                Name = "Surprise attack!",
                Description = "They never saw it comming",
                Attack = 5,
                Defense = 0,
                Cost = 3,
                ImagePath = "3.jpg"

            });

            cards.Add(new Card
            {
                Name = "Inner power!",
                Description = "Dispenses the power from within",
                Attack = 10,
                Defense = 5,
                Cost = 8,
                ImagePath = "4.jpg"

            });

            cards.Add(new Card
            {
                Name = "Darkness within",
                Description = "Sacrafices defend for high power",
                Attack = 12,
                Defense = -3,
                Cost = 9,
                ImagePath = "5.jpg"

            });

            cards.Add(new Card
            {
                Name = "Power of Purr-ship",
                Description = "Power of friendship!",
                Attack = 12,
                Defense = 8,
                Cost = 10,
                ImagePath = "6.jpg"

            });

            cards.Add(new Card
            {
                Name = "Apprentice",
                Description = "The apprentice rises",
                Attack = 4,
                Defense = 2,
                Cost = 3,
                ImagePath = "7.jpg"

            });

            cards.Add(new Card
            {
                Name = "The guard",
                Description = "The Cat army guard",
                Attack = 2,
                Defense = 5,
                Cost = 2,
                ImagePath = "8.jpg"

            });

            cards.Add(new Card
            {
                Name = "The Magistar",
                Description = "The wandering Magistar",
                Attack = 6,
                Defense = 2,
                Cost = 5,
                ImagePath = "9.jpg"

            });

            return cards;
        }
    }
}
