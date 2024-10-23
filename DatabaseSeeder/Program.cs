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
            var enemies = Reader<Enemy>("../../../Enemies.json", "Enemies");
            return enemies;
        }

        private static List<Card> GenerateCards()
        {
            var cards = Reader<Card>("../../../Cards.json", "Cards");
            return cards;
        }

        private static List<T> Reader<T>(string filePath, string type)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            string jsonString = File.ReadAllText(filePath);
            var jsonDoc = JsonDocument.Parse(jsonString);

            if (type == "Cards" || type == "Enemies")
            {
                var elements = jsonDoc.RootElement.GetProperty(type);
                return JsonSerializer.Deserialize<List<T>>(elements.ToString(), options);
            }
            else
            {
                throw new Exception("NOT A VALID OBJECT TYPE");
            }
        }
    }
}
