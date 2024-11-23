using Domain.DTOs;
using Infrastructure.Persistance.Document;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadOfNineLives.IntegrationTests
{
    public class DocumentDBCardTests
    {
        private readonly DocumentContext _context;
        public DocumentDBCardTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("dbsettings.json")
                .Build();
            
            var settings = configuration.GetSection("ConnectionStrings:MongoDB");
            var connectionString = settings.GetSection("Connectionstring").Value;
            var databaseName = settings.GetSection("DatabaseName").Value;

            _context = new DocumentContext(connectionString, databaseName);

        }

        [Fact]
        public void UpdateCardShouldUpdateDecks()
        {
            //Assign
            // create data for the test, in this case, a card and a deck
            var card = new CardDTO()
            {
                Attack = 1,
                Cost = 1,
                Defence = 1,
                Description = "Firsttest",
                ImagePath = "Firsttest",
                Name = "FirstName",
            };
            var 

            //Act

            //Assert
        }
    }
}
