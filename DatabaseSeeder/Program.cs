using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Domain.Entities;
using Infrastructure.Persistance.Relational;
using Microsoft.Extensions.Configuration;
using Infrastructure.Persistance;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using System;


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
