using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Domain.Entities;
using Infrastructure.Persistance.Relational;
using Microsoft.Extensions.Configuration;
using Infrastructure.Persistance;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using System;
using Microsoft.EntityFrameworkCore.Migrations.Internal;


namespace DataSeeder
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var services = new ServiceCollection();
            PersistanceConfiguration.ConfigureServices(services, dbtype.DefaultConnection);

            using (var serviceProvider = services.BuildServiceProvider())
            {
                Console.WriteLine("Migrating Database..");
                Migrate(serviceProvider);
                Console.WriteLine("Seeding Database..");
                SeedDatabase(serviceProvider);
                Console.WriteLine("Done!");
            }
        }
        private static void Migrate(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<RelationalContext>();
                context.Database.Migrate();
            }
        }

        private static void SeedDatabase(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
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
            var enemies = Reader<List<Enemy>>("Enemies.json");
            return enemies;
        }

        private static List<Card> GenerateCards()
        {
            var cards = Reader<List<Card>>("Cards.json");
            return cards;
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
