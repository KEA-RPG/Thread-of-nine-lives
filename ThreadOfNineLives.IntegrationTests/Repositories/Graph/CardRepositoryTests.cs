using Backend.Repositories.Document;
using Backend.Repositories.Graph;
using Backend.Repositories.Interfaces;
using Domain.DTOs;
using Infrastructure.Persistance;
using Infrastructure.Persistance.Document;

namespace ThreadOfNineLives.IntegrationTests.Repositories.Graph
{
    public class CardRepositoryTests
    {
        private readonly ICardRepository _cardRepository;
        private readonly IDeckRepository _deckRepository;
        private readonly IUserRepository _userRepository;
        public CardRepositoryTests()
        {

            var _context = PersistanceConfiguration.GetGraphContext(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));

            _cardRepository = new GraphCardRepository(_context);
            _deckRepository = new GraphDeckRepository(_context);
            _userRepository = new GraphUserRepository(_context);

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
            var insertedCard = _cardRepository.AddCard(card);

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
            //Arrange & Act
            var card = _cardRepository.GetCardById(-1);

            //Assert
            Assert.Null(card);
        }

        [Fact]
        public void GetCardById_Returns_Item()
        {
            //Arrange
            var deck = CreateTemplateDecksAndCards();
            var cardId = deck.Cards.First().Id;
            //Act
            var card = _cardRepository.GetCardById(cardId);

            //Assert
            Assert.NotNull(card);
        }
        [Fact]
        public void UpdateCard_Should_Update_Database_Card()
        {
            //Arrange
            var deck = CreateTemplateDecksAndCards();
            var cardToUpdateId = deck.Cards.First().Id;
            var cardToUpdate = _cardRepository.GetCardById(cardToUpdateId);
            cardToUpdate.Name = "UpdatedName";
            cardToUpdate.Defence = 123456789;
            cardToUpdate.Attack = 123456789;
            cardToUpdate.Cost = 123456789;
            cardToUpdate.Description = "123456789";
            cardToUpdate.ImagePath = "123456789";

            //Act
            _cardRepository.UpdateCard(cardToUpdate);
            var updatedCard = _cardRepository.GetCardById(cardToUpdateId);

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
            var deck = CreateTemplateDecksAndCards();
            var cardToDelete = deck.Cards.First();

            //Act
            _cardRepository.DeleteCard(cardToDelete);
            var updatedDeck = _deckRepository.GetDeckById(deck.Id);

            //Assert
            Assert.DoesNotContain(updatedDeck.Cards, x => x.Id == cardToDelete.Id);
        }


        [Fact]
        public void Delete_Card_Should_Remove_From_Collection()
        {
            //Arrange
            var deck = CreateTemplateDecksAndCards();
            var cardToDelete = deck.Cards.First();

            //Act
            _cardRepository.DeleteCard(cardToDelete);
            var card = _cardRepository.GetCardById(cardToDelete.Id);

            //Assert
            Assert.Null(card);
        }

        [Fact]
        public void GetAllCards_Returns_Items()
        {
            //Arrange
            CreateTemplateDecksAndCards();

            //Act
            var cards = _cardRepository.GetAllCards();

            //Assert
            Assert.NotEmpty(cards);


        }

        private DeckDTO CreateTemplateDecksAndCards()
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
            var cardList = new List<CardDTO>();
            cardList.Add(_cardRepository.AddCard(card1));
            cardList.Add(_cardRepository.AddCard(card2));

            var user = new UserDTO()
            {
                Password = "password",
                Role = "Player",
                Username = "username1INTEGRATION",
            };
            _userRepository.CreateUser(user);
            var dbuser = _userRepository.GetUserByUsername(user.Username);
            var deck = new DeckDTO()
            {
                Name = "deck1INTEGRATION",
                IsPublic = true,
                UserId = dbuser.Id,
                UserName = dbuser.Username,
                Cards = cardList.ToList(),
            };

            return _deckRepository.AddDeck(deck);
        }
    }
}
