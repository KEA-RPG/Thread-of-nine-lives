using Backend.Repositories.Document;
using Domain.DTOs;
using Infrastructure.Persistance.Document;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadOfNineLives.IntegrationTests.DocumentDB
{
    public class DocumentDeckRepositoryTest : IDisposable
    {
        private readonly DocumentContext _context;
        private readonly MongoDeckRepository _mongoDeckRepository;
        private readonly DatabaseSnapshotHelper _snapshotHelper;
        public DocumentDeckRepositoryTest()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("dbsettings.json")
                .Build();

            var settings = configuration.GetSection("ConnectionStrings:MongoDB");
            var connectionString = settings.GetSection("Connectionstring").Value;
            var databaseName = settings.GetSection("DatabaseName").Value;

            _context = new DocumentContext(connectionString, databaseName);
            _mongoDeckRepository = new MongoDeckRepository(_context);
            _snapshotHelper = new DatabaseSnapshotHelper(_context);

            _snapshotHelper.TakeSnapshot();
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
            var data = _mongoDeckRepository.AddDeck(testDeck);

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
            _mongoDeckRepository.AddComment(comment);
            var dbDeck = _mongoDeckRepository.GetDeckById(deck.Id);

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
            _mongoDeckRepository.AddComment(comment);
            _mongoDeckRepository.AddComment(comment2);
            var dbDeck = _mongoDeckRepository.GetDeckById(deck.Id);

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
            _mongoDeckRepository.DeleteDeck(preDeck.Id);
            var deck = _mongoDeckRepository.GetDeckById(preDeck.Id);

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
            _mongoDeckRepository.AddComment(comment);
            _mongoDeckRepository.AddComment(comment2);
            var comments = _mongoDeckRepository.GetCommentsByDeckId(deck.Id);
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
            var dbDeck = _mongoDeckRepository.GetDeckById(deck.Id);

            //Assert
            Assert.NotNull(dbDeck);
        }

        [Fact]
        public void GetDeckById_Returns_With_Invalid_Id()
        {
            // Arrange
            _context.Decks().DeleteMany(FilterDefinition<DeckDTO>.Empty);

            // Act
            var retrievedDeck = _mongoDeckRepository.GetDeckById(9999);

            // Assert
            Assert.Null(retrievedDeck);
        }

        [Fact]
        public void GetPublicDecks_Returns_Valid_Data()
        {
            // Arrange
            CreateTemplateDeck();

            // Act
            var retrievedDecks = _mongoDeckRepository.GetPublicDecks();

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
            _mongoDeckRepository.AddDeck(testDeck);

            //Act
            var data = _mongoDeckRepository.GetUserDecks("123");

            //Assert
            Assert.True(data.Any());
            Assert.Single(data);

        }

        [Fact]
        public void GetUserDecks_Returns_No_Decks_For_Invalid_Username()
        {
            //Arrange
            _context.Decks().DeleteMany(FilterDefinition<DeckDTO>.Empty);

            //Act
            var data = _mongoDeckRepository.GetUserDecks("123");

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
            _mongoDeckRepository.UpdateDeck(deck);
            var data = _mongoDeckRepository.GetDeckById(deck.Id);

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
            _mongoDeckRepository.UpdateDeck(deck);
            var data = _mongoDeckRepository.GetDeckById(deck.Id);

            //Arrange
            Assert.True(data.Name == deck.Name);
        }


        public void Dispose()
        {
            _snapshotHelper.RestoreSnapshot();
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
            var data = _mongoDeckRepository.AddDeck(testDeck);
            return data;
        }
    }
}
