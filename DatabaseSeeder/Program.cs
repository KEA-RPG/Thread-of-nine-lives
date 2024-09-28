using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Domain.Entities;
using Infrastructure.Persistance.Relational;
using System;
using System.Collections.Generic;

namespace DataSeeder
{

    class Program
    {
        public static string ConnectionString; //TODO: Set the connection string

        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            SeedDatabase(host);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContext<RelationalContext>(options =>
                        options.UseSqlServer(ConnectionString));
                });

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
            }
        }

        private static List<Enemy> GenerateEnemies()
        {
            var enemies = new List<Enemy>();

            enemies.Add(new Enemy
            {
                Id = 1,
                Name = "Rat",
                Health = "10",
                ImagePath = "rat.jpg"
            });

            enemies.Add(new Enemy
            {
                Id = 2,
                Name = "Goblin",
                Health = "15",
                ImagePath = "goblin.jpg"
            });

            enemies.Add(new Enemy
            {
                Id = 3,
                Name = "Orc",
                Health = "20",
                ImagePath = "orc.jpg"
            });

            enemies.Add(new Enemy
            {
                Id = 4,
                Name = "Troll",
                Health = "25",
                ImagePath = "troll.jpg"
            });

            enemies.Add(new Enemy
            {
                Id = 5,
                Name = "Dragon",
                Health = "30",
                ImagePath = "dragon.jpg"
            });

            enemies.Add(new Enemy
            {
                Id = 6,
                Name = "Wraith",
                Health = "18",
                ImagePath = "wraith.jpg"
            });

            enemies.Add(new Enemy
            {
                Id = 7,
                Name = "Minotaur",
                Health = "28",
                ImagePath = "minotaur.jpg"
            });

            enemies.Add(new Enemy
            {
                Id = 8,
                Name = "Harpy",
                Health = "22",
                ImagePath = "harpy.jpg"
            });

            enemies.Add(new Enemy
            {
                Id = 9,
                Name = "Skeleton",
                Health = "15",
                ImagePath = "skeleton.jpg"
            });

            enemies.Add(new Enemy
            {
                Id = 10,
                Name = "Vampire",
                Health = "35",
                ImagePath = "vampire.jpg"
            });

            return enemies;
        }

        private static List<Card> GenerateCards()
        {
            var cards = new List<Card>();

            // Generate cards here
            cards.Add(new Card
            {
                Id = 1,
                Name = "First battle!",
                Description = "It's this little fellas first battle! He can aid in hitting, but have not yet learned to defend himself",
                Attack = 3,
                Defense = 0,
                Cost = 0,
                ImagePath = "1.jpg"
            });

            cards.Add(new Card
            {
                Id = 2,
                Name = "Guidance",
                Description = "With the guidence of his mentor, he is now a little bit more sturdy",
                Attack = 3,
                Defense = 2,
                Cost = 1,
                ImagePath = "2.jpg"

            });

            cards.Add(new Card
            {
                Id = 3,
                Name = "Surprise attack!",
                Description = "They never saw it comming",
                Attack = 5,
                Defense = 0,
                Cost = 3,
                ImagePath = "3.jpg"

            });

            cards.Add(new Card
            {
                Id = 4,
                Name = "Inner power!",
                Description = "Dispenses the power from within",
                Attack = 10,
                Defense = 5,
                Cost = 8,
                ImagePath = "4.jpg"

            });

            cards.Add(new Card
            {
                Id = 5,
                Name = "Darkness within",
                Description = "Sacrafices defend for high power",
                Attack = 12,
                Defense = -3,
                Cost = 9,
                ImagePath = "5.jpg"

            });

            cards.Add(new Card
            {
                Id = 6,
                Name = "Power of Purr-ship",
                Description = "Power of friendship!",
                Attack = 12,
                Defense = 8,
                Cost = 10,
                ImagePath = "6.jpg"

            });

            cards.Add(new Card
            {
                Id = 7,
                Name = "Apprentice",
                Description = "The apprentice rises",
                Attack = 4,
                Defense = 2,
                Cost = 3,
                ImagePath = "7.jpg"

            });

            cards.Add(new Card
            {
                Id = 8,
                Name = "The guard",
                Description = "The Cat army guard",
                Attack = 2,
                Defense = 5,
                Cost = 2,
                ImagePath = "8.jpg"

            });

            cards.Add(new Card
            {
                Id = 9,
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
