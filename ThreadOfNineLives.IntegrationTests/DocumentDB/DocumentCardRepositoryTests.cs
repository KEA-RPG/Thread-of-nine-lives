using Backend.Repositories.Document;
using Domain.DTOs;
using Infrastructure.Persistance.Document;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;

namespace ThreadOfNineLives.IntegrationTests.DocumentDB
{
    public class DocumentCardRepositoryTests : IDisposable
    {
        private readonly DocumentContext _context;
        private readonly MongoCardRepository _mongoCardRepository;
        private readonly MongoDeckRepository _mongoDeckRepository;
        private readonly MongoUserRepository _mongoUserRepository;
        private readonly DatabaseSnapshotHelper _snapshotHelper;
        public DocumentCardRepositoryTests()
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
        public void AddCard_Assigns_Id_And_Retains_Values()
        {
            // Arrange
            var card = new CardDTO()
            {
                Attack = 5,
                Cost = 3,
                Defence = 2,
                Description = "TestDescription",
                ImagePath = "TestImagePath",
                Name = "TestCardName",
            };

            // Act
            var insertedCard = _mongoCardRepository.AddCard(card);

            // Assert
            Assert.NotNull(insertedCard);
            Assert.True(insertedCard.Id > 0);
            Assert.Equal(5, insertedCard.Attack);
            Assert.Equal(3, insertedCard.Cost);
            Assert.Equal(2, insertedCard.Defence);
            Assert.Equal("TestDescription", insertedCard.Description);
            Assert.Equal("TestImagePath", insertedCard.ImagePath);
            Assert.Equal("TestCardName", insertedCard.Name);
        }
        [Fact]
        public void GetCardById_Returns_Empty_When_Invalid_ID()
        {
            //Arrange
            _context.Cards().DeleteMany(FilterDefinition<CardDTO>.Empty);

            //Act
            var card = _mongoCardRepository.GetCardById(10000);

            //Assert
            Assert.Null(card);
        }

        [Fact]
        public void GetCardById_Returns_Item()
        {
            //Arrange
            CreateTemplateDecksAndCards();
            var initialCardName = "FirstNameINTEGRATION";
            var cardId = _context.Cards().Find(x => x.Name == initialCardName).First().Id;

            //Act
            var card = _mongoCardRepository.GetCardById(cardId);

            //Assert
            Assert.NotNull(card);
        }
        [Fact]
        public void UpdateCard_Should_Update_Database_Card()
        {

            //Arrange
            CreateTemplateDecksAndCards();
            var initialCardName = "FirstNameINTEGRATION";
            var cardToUpdateId = _context.Cards().Find(x => x.Name == initialCardName).First().Id;
            var cardToUpdate = _mongoCardRepository.GetCardById(cardToUpdateId);
            cardToUpdate.Name = "UpdatedName";
            cardToUpdate.Defence = 123456789;
            cardToUpdate.Attack = 123456789;
            cardToUpdate.Cost = 123456789;
            cardToUpdate.Description = "123456789";
            cardToUpdate.ImagePath = "123456789";

            //Act
            _mongoCardRepository.UpdateCard(cardToUpdate);
            var updatedCard = _mongoCardRepository.GetCardById(cardToUpdateId);

            //Assert
            Assert.True(updatedCard.Name == "UpdatedName");
            Assert.True(updatedCard.Defence == 123456789);
            Assert.True(updatedCard.Attack == 123456789);
            Assert.True(updatedCard.Cost == 123456789);
            Assert.True(updatedCard.Description == "123456789");
            Assert.True(updatedCard.ImagePath == "123456789");
        }

        [Fact]
        public void Delete_Card_Should_Update_Decks()
        {
            //Arrange
            CreateTemplateDecksAndCards();
            var initialCardName = "FirstNameINTEGRATION";
            var cardToDelete = _context.Cards().Find(x => x.Name == initialCardName).First();

            //Act
            _mongoCardRepository.DeleteCard(cardToDelete);
            var deck = _context.Decks().Find(x => x.Name == "deck1INTEGRATION").First();

            //Assert
            Assert.False(deck.Cards.Any(x => x.Id == cardToDelete.Id));
        }


        [Fact]
        public void Delete_Card_Should_Remove_From_Collection()
        {
            //Arrange
            CreateTemplateDecksAndCards();
            var initialCardName = "FirstNameINTEGRATION";
            var cardToDelete = _context.Cards().Find(x => x.Name == initialCardName).First();

            //Act
            _mongoCardRepository.DeleteCard(cardToDelete);
            var card = _context.Cards().Find(x => x.Id == cardToDelete.Id).FirstOrDefault();

            //Assert
            Assert.Null(card);
        }

        [Fact]
        public void GetAllCards_Returns_Items()
        {
            //Arrange
            CreateTemplateDecksAndCards();

            //Act
            var cards = _mongoCardRepository.GetAllCards();

            //Assert
            Assert.NotEmpty(cards);


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
