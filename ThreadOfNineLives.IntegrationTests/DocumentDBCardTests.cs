using Backend.Repositories.Document;
using Domain.DTOs;
using Infrastructure.Persistance.Document;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Linq;

namespace ThreadOfNineLives.IntegrationTests
{
    public class DocumentDBCardTests : IDisposable
    {
        private readonly DocumentContext _context;
        private readonly MongoCardRepository _mongoCardRepository;
        private readonly MongoDeckRepository _mongoDeckRepository;
        private readonly MongoUserRepository _mongoUserRepository;
        private readonly DatabaseSnapshotHelper _snapshotHelper;
        public DocumentDBCardTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("dbsettings.json")
                .Build();
            
            var settings = configuration.GetSection("ConnectionStrings:MongoDB");
            var connectionString = settings.GetSection("Connectionstring").Value;
            var databaseName = settings.GetSection("DatabaseName").Value;

            _context = new DocumentContext(connectionString, databaseName);
            _mongoCardRepository = new MongoCardRepository(_context);
            _mongoDeckRepository = new MongoDeckRepository(_context);
            _mongoUserRepository = new MongoUserRepository(_context);
            _snapshotHelper = new DatabaseSnapshotHelper(_context);

            _snapshotHelper.TakeSnapshot();
        }

        [Fact]
        public void UpdateCardShouldUpdateDecks()
        {

            //Assign
            CreateTemplateDecksAndCards();
            var initialCardName = "FirstNameINTEGRATION";
            var cardToUpdate = _context.Cards().Find(x => x.Name == initialCardName).First();
            cardToUpdate.Name = "ThirdNameINTEGRATION";

            //Act
            _mongoCardRepository.UpdateCard(cardToUpdate);
            var deck = _context.Decks().Find(x => x.Name == "deck1INTEGRATION").First();
            var updatedCard = deck.Cards.First(x => x.Id == cardToUpdate.Id);

            //Assert
            Assert.NotEqual(initialCardName, updatedCard.Name);
        }

        public void Dispose()
        {
            _snapshotHelper.RestoreSnapshot();
        }
        private void CreateTemplateDecksAndCards()
        {
            var card1 = new CardDTO()
            {
                Attack = 1,
                Cost = 1,
                Defence = 1,
                Description = "FirstNameINTEGRATION",
                ImagePath = "FirstNameINTEGRATION",
                Name = "FirstNameINTEGRATION",
            };

            var card2 = new CardDTO()
            {
                Attack = 10,
                Cost = 10,
                Defence = 10,
                Description = "SecondtestINTEGRATION",
                ImagePath = "SecondtestINTEGRATION",
                Name = "SecondtestINTEGRATION",
            };
            _mongoCardRepository.AddCard(card1);
            _mongoCardRepository.AddCard(card2);

            var user = new UserDTO()
            {
                Password = "password",
                Role = "Player",
                Username = "username1INTEGRATION",
            };
            _mongoUserRepository.CreateUser(user);
            var dbuser = _context.Users().Find(x => x.Username.Contains("INTEGRATION")).First();
            var dbCards = _context.Cards().Find(x => x.Name.Contains("INTEGRATION")).ToList(); ;
            var deck = new DeckDTO()
            {
                Name = "deck1INTEGRATION",
                IsPublic = true,
                UserId = dbuser.Id,
                UserName = dbuser.Username,
                Cards = dbCards.ToList(),
            };

            _mongoDeckRepository.AddDeck(deck);
        }

    }
}
