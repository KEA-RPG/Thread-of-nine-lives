using Backend.Repositories.Document;
using Backend.Repositories.Interfaces;
using Domain.DTOs;
using Infrastructure.Persistance;

namespace ThreadOfNineLives.IntegrationTests.Repositories.DocumentDB
{
    public class DeckRepositoryTest
    {
        private readonly IDeckRepository _deckRepository;
        public DeckRepositoryTest()
        {
            var _context = PersistanceConfiguration.GetDocumentContext(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
            _deckRepository = new MongoDeckRepository(_context);
        }

        [Fact]
        public void AddDeck_Assigns_Id_And_Retains_Values()
        {
            //Arrange
            var testDeck = new DeckDTO()
            {
                Cards = new List<CardDTO>(),
                Comments = new List<CommentDTO>(),
                IsPublic = true,
                Name = "Test",
                UserId = 123,
                UserName = "Test username",
            };

            //Act
            var data = _deckRepository.AddDeck(testDeck);

            // Assert
            Assert.NotNull(data);
            Assert.True(data.Id > 0);
            Assert.Equal(testDeck.Cards, data.Cards);
            Assert.Equal(testDeck.Comments, data.Comments);
            Assert.Equal(testDeck.IsPublic, data.IsPublic);
            Assert.Equal(testDeck.Name, data.Name);
            Assert.Equal(testDeck.UserId, data.UserId);
            Assert.Equal(testDeck.UserName, data.UserName);

        }

        [Fact]
        public void AddComment_Adds_Comment_To_Deck()
        {
            //Arrange
            var deck = CreateTemplateDeck();
            var comment = new CommentDTO()
            {
                CreatedAt = DateTime.UtcNow,
                DeckId = deck.Id,
                Text = "Test comment",
                UserId = 123,
                Username = "TestCommentor",
            };

            //Act
            _deckRepository.AddComment(comment);
            var dbDeck = _deckRepository.GetDeckById(deck.Id);

            //Assert
            Assert.NotNull(dbDeck);
            Assert.True(dbDeck.Comments.Count == 1);

        }

        [Fact]
        public void AddComment_Twice_Adds_Comments_To_Deck()
        {
            //Arrange
            var deck = CreateTemplateDeck();
            var comment = new CommentDTO()
            {
                CreatedAt = DateTime.UtcNow,
                DeckId = deck.Id,
                Text = "Test comment",
                UserId = 123,
                Username = "TestCommentor",
            };
            var comment2 = new CommentDTO()
            {
                CreatedAt = DateTime.UtcNow,
                DeckId = deck.Id,
                Text = "Test comment",
                UserId = 123,
                Username = "TestCommentor",
            };

            //Act
            _deckRepository.AddComment(comment);
            _deckRepository.AddComment(comment2);
            var dbDeck = _deckRepository.GetDeckById(deck.Id);

            //Assert
            Assert.NotNull(dbDeck);
            Assert.True(dbDeck.Comments.Count == 2);
        }

        [Fact]
        public void Delete_Deck_Should_Remove_From_Collection()
        {
            //Arrange
            var preDeck = CreateTemplateDeck();

            //Act
            _deckRepository.DeleteDeck(preDeck.Id);
            var deck = _deckRepository.GetDeckById(preDeck.Id);

            //Assert
            Assert.Null(deck);
        }

        [Fact]
        public void GetCommentsByDeckId_Returns_Comments()
        {
            //Arrange
            var deck = CreateTemplateDeck();
            var comment = new CommentDTO()
            {
                CreatedAt = DateTime.UtcNow,
                DeckId = deck.Id,
                Text = "Test comment",
                UserId = 123,
                Username = "TestCommentor",
            };
            var comment2 = new CommentDTO()
            {
                CreatedAt = DateTime.UtcNow,
                DeckId = deck.Id,
                Text = "Test comment",
                UserId = 123,
                Username = "TestCommentor",
            };

            //Act
            _deckRepository.AddComment(comment);
            _deckRepository.AddComment(comment2);
            var comments = _deckRepository.GetCommentsByDeckId(deck.Id);
            //Assert
            Assert.NotNull(comments);
            Assert.True(comments.Count == 2);
        }

        [Fact]
        public void GetDeckById_Returns_With_Valid_Id()
        {
            //Arrange
            var deck = CreateTemplateDeck();

            //Act
            var dbDeck = _deckRepository.GetDeckById(deck.Id);

            //Assert
            Assert.NotNull(dbDeck);
        }

        [Fact]
        public void GetDeckById_Returns_With_Invalid_Id()
        {
            // Arrange & Act
            var retrievedDeck = _deckRepository.GetDeckById(-1);

            // Assert
            Assert.Null(retrievedDeck);
        }

        [Fact]
        public void GetPublicDecks_Returns_Valid_Data()
        {
            // Arrange
            CreateTemplateDeck();

            // Act
            var retrievedDecks = _deckRepository.GetPublicDecks();

            // Assert
            Assert.NotNull(retrievedDecks);
            Assert.True(retrievedDecks.Count() > 0);
        }

        [Fact]
        public void GetUserDecks_Returns_Decks_For_Valid_Username()
        {
            //Arrange
            var testDeck = new DeckDTO()
            {
                Cards = new List<CardDTO>(),
                Comments = new List<CommentDTO>(),
                IsPublic = true,
                Name = "Test",
                UserId = 123,
                UserName = "123",
            };
            _deckRepository.AddDeck(testDeck);

            //Act
            var data = _deckRepository.GetUserDecks("123");

            //Assert
            Assert.True(data.Any());
        }

        [Fact]
        public void GetUserDecks_Returns_No_Decks_For_Invalid_Username()
        {
            // Arrange & Act
            var data = _deckRepository.GetUserDecks("-1");

            //Assert
            Assert.Empty(data);

        }

        [Fact]
        public void UpdateDeck_Does_Not_Update_Comments()
        {
            //Arrange
            var deck = CreateTemplateDeck();
            var comment = new CommentDTO()
            {
                CreatedAt = DateTime.UtcNow,
                DeckId = deck.Id,
                Text = "Test comment",
                UserId = 123,
                Username = "TestCommentor",
            };
            deck.Comments.Add(comment);

            //Act
            _deckRepository.UpdateDeck(deck);
            var data = _deckRepository.GetDeckById(deck.Id);

            //Arrange
            Assert.Empty(data.Comments);
        }

        [Fact]
        public void UpdateDeck_Updates_Name()
        {
            //Arrange
            var deck = CreateTemplateDeck();
            deck.Name = "NEWNAME";
            //Act
            _deckRepository.UpdateDeck(deck);
            var data = _deckRepository.GetDeckById(deck.Id);

            //Arrange
            Assert.True(data.Name == deck.Name);
        }

        private DeckDTO CreateTemplateDeck()
        {
            var testDeck = new DeckDTO()
            {
                Cards = new List<CardDTO>(),
                Comments = new List<CommentDTO>(),
                IsPublic = true,
                Name = "Test",
                UserId = 123,
                UserName = "Test username",
            };

            //Act
            var data = _deckRepository.AddDeck(testDeck);
            return data;
        }
    }
}
