using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moq;
using Backend.Services;
using Domain.DTOs;
using Domain.Entities;
using Backend.Repositories.Interfaces;
using Backend.SecurityLogic; 
using System.Reflection;

namespace Backend.Tests
{
    public class DeckServiceTests
    {
        private readonly Mock<IDeckRepository> _mockDeckRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly DeckService _deckService;

        public DeckServiceTests()
        {
            _mockDeckRepository = new Mock<IDeckRepository>();
            _mockUserRepository = new Mock<IUserRepository>();

            _deckService = new DeckService(_mockDeckRepository.Object, _mockUserRepository.Object);
        }

        [Fact]
        public void CreateDeck_ShouldCallRepositoryAddDeck_AndReturnNewDeck()
        {
            // Arrange
            var inputDeck = new DeckDTO
            {
                Id = 0, 
                Name = "New Deck"
            };
            var outputDeck = new DeckDTO
            {
                Id = 10,
                Name = "New Deck"
            };

            _mockDeckRepository
                .Setup(repo => repo.AddDeck(inputDeck))
                .Returns(outputDeck);

            // Act
            var result = _deckService.CreateDeck(inputDeck);

            // Assert
            Assert.Equal(10, result.Id);
            Assert.Equal(inputDeck.Name, result.Name);
            _mockDeckRepository.Verify(repo => repo.AddDeck(inputDeck), Times.Once);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(999)]
        public void DeleteDeck_ShouldThrowKeyNotFound_WhenDeckDoesNotExist(int id)
        {
            // Arrange
            _mockDeckRepository
                .Setup(repo => repo.GetDeckById(id))
                .Returns((DeckDTO)null);

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _deckService.DeleteDeck(id));
        }

        [Fact]
        public void DeleteDeck_ShouldCallDeleteDeck_WhenDeckExists()
        {
            // Arrange
            int deckId = 10;
            var existingDeck = new DeckDTO { Id = deckId, Name = "Existing Deck" };

            _mockDeckRepository
                .Setup(repo => repo.GetDeckById(deckId))
                .Returns(existingDeck);

            // Act
            _deckService.DeleteDeck(deckId);

            // Assert
            _mockDeckRepository.Verify(repo => repo.DeleteDeck(deckId), Times.Once);
        }

        [Fact]
        public void GetDeckById_ShouldThrowKeyNotFound_WhenNullReturnedFromRepository()
        {
            // Arrange
            int deckId = 123;
            _mockDeckRepository
                .Setup(repo => repo.GetDeckById(deckId))
                .Returns((DeckDTO)null);

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _deckService.GetDeckById(deckId));
        }

        [Fact]
        public void GetDeckById_ShouldReturnDeck_WhenDeckExists()
        {
            // Arrange
            int deckId = 20;
            var deckDto = new DeckDTO { Id = deckId, Name = "Test Deck" };

            _mockDeckRepository
                .Setup(repo => repo.GetDeckById(deckId))
                .Returns(deckDto);

            // Act
            var result = _deckService.GetDeckById(deckId);

            // Assert
            Assert.Equal(deckId, result.Id);
            Assert.Equal(deckDto.Name, result.Name);
        }

        [Fact]
        public void GetUserDecks_ShouldReturnListOfDecks()
        {
            // Arrange
            string userName = "testUser";
            var userDecks = new List<DeckDTO>
    {
        new DeckDTO { Id = 1, Name = "Deck A" },
        new DeckDTO { Id = 2, Name = "Deck B" }
    };
            _mockDeckRepository
                .Setup(repo => repo.GetUserDecks(userName))
                .Returns(userDecks);

            // Act
            var result = _deckService.GetUserDecks(userName);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(result[0].Name, "Deck A");
            Assert.Equal(result[1].Name, "Deck B");
        }


        [Fact]
        public void GetUserDecks_ShouldReturnEmptyCollectionIfNoDecksFound()
        {
            // Arrange
            string userName = "nobody";
            _mockDeckRepository
                .Setup(repo => repo.GetUserDecks(userName))
                .Returns(new List<DeckDTO>());

            // Act
            var result = _deckService.GetUserDecks(userName);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }


        [Fact]
        public void UpdateDeck_ShouldCallRepositoryUpdateDeck_AndReturnUpdatedDeck()
        {
            // Arrange
            var deckToUpdate = new DeckDTO
            {
                Id = 100,
                Name = "Updated Name"
            };

            var updatedDeck = new DeckDTO
            {
                Id = deckToUpdate.Id,
                Name = deckToUpdate.Name,
                IsPublic = true
            };

            _mockDeckRepository
                .Setup(repo => repo.GetDeckById(deckToUpdate.Id))
                .Returns(updatedDeck);

            // Act
            var result = _deckService.UpdateDeck(deckToUpdate);

            // Assert
            _mockDeckRepository.Verify(repo => repo.UpdateDeck(deckToUpdate), Times.Once);
            Assert.Equal(deckToUpdate.Name, result.Name);
            Assert.True(result.IsPublic);
        }


        [Fact]
        public void GetPublicDecks_ShouldReturnListOfPublicDecks()
        {
            // Arrange
            var publicDecks = new List<DeckDTO>
    {
        new DeckDTO { Id = 1, Name = "Public Deck 1", IsPublic = true },
        new DeckDTO { Id = 2, Name = "Public Deck 2", IsPublic = true }
    };

            _mockDeckRepository
                .Setup(repo => repo.GetPublicDecks())
                .Returns(publicDecks);

            // Act
            var result = _deckService.GetPublicDecks();

            // Assert
            Assert.Equal(publicDecks.Count, result.Count);
            Assert.Equal(result[0].Name, "Public Deck 1");
            Assert.Equal(result[1].Name, "Public Deck 2");
            Assert.True(result[0].IsPublic);
            Assert.True(result[1].IsPublic);
        }

        [Fact]
        public void GetPublicDecks_ShouldReturnEmptyList_WhenNoPublicDecks()
        {
            // Arrange
            _mockDeckRepository
                .Setup(repo => repo.GetPublicDecks())
                .Returns(new List<DeckDTO>());

            // Act
            var result = _deckService.GetPublicDecks();

            // Assert
            Assert.Empty(result);
        }


        [Fact]
        public void AddComment_ShouldThrow_KeyNotFound_WhenDeckDoesNotExist()
        {
            //Arange
            var commentDto = new CommentDTO { DeckId = 1, Username = "someUser" };

            //Act
            _mockDeckRepository
                .Setup(repo => repo.GetDeckById(commentDto.DeckId))
                .Returns((DeckDTO)null);

            //Assert
            Assert.Throws<KeyNotFoundException>(() => _deckService.AddComment(commentDto));
        }

        [Fact]
        public void AddComment_ShouldThrow_KeyNotFound_WhenUserDoesNotExist()

        {//Arange
            var commentDto = new CommentDTO { DeckId = 1, Username = "non_existent_user" };

         //Act
            _mockDeckRepository
                .Setup(repo => repo.GetDeckById(commentDto.DeckId))
                .Returns(new DeckDTO { Id = commentDto.DeckId });

            _mockUserRepository
                .Setup(repo => repo.GetUserByUsername("non_existent_user"))
                .Returns((UserDTO)null);

         //Assert
            Assert.Throws<KeyNotFoundException>(() => _deckService.AddComment(commentDto));
        }


        [Fact]
        public void GetCommentsByDeckId_ShouldThrowException_IfNoCommentsFound()
        {
            // Arrange
            int deckId = 1;

            _mockDeckRepository
                .Setup(repo => repo.GetDeckById(deckId))
                .Returns(new DeckDTO { Id = deckId, Name = "Some Deck" });

            _mockDeckRepository
                .Setup(repo => repo.GetCommentsByDeckId(deckId))
                .Returns(new List<CommentDTO>()); // empty

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _deckService.GetCommentsByDeckId(deckId));
            Assert.Contains("No comments found for Deck with ID 1", ex.Message);
        }

        [Fact]
        public void GetCommentsByDeckId_ShouldThrowException_WhenDeckDoesNotExist()
        {
            // Arrange
            var deckId = 1;
            _mockDeckRepository.Setup(repo => repo.GetDeckById(deckId)).Returns((DeckDTO)null);

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _deckService.GetCommentsByDeckId(deckId));
        }

        [Fact]
        public void AddComment_ShouldThrowException_WhenDeckDoesNotExist()
        {
            // Arrange
            var deckId = 1;
            var commentDto = new CommentDTO
            {
                Id = 1,
                DeckId = deckId,
                Text = "Great deck!",
                CreatedAt = DateTime.UtcNow,
                Username = "test_user"
            };

            _mockDeckRepository.Setup(repo => repo.GetDeckById(deckId)).Returns((DeckDTO)null);

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _deckService.AddComment(commentDto));
        }

        [Fact]
        public void AddComment_ShouldThrowException_WhenUserDoesNotExist()
        {
            // Arrange
            var deckId = 1;
            var username = "non_existent_user";
            var commentDto = new CommentDTO
            {
                Id = 1,
                DeckId = deckId,
                Text = "Great deck!",
                CreatedAt = DateTime.UtcNow,
                Username = username
            };

            _mockDeckRepository.Setup(repo => repo.GetDeckById(deckId)).Returns(new DeckDTO { Id = deckId });
            _mockUserRepository.Setup(repo => repo.GetUserByUsername(username)).Returns((UserDTO)null);

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _deckService.AddComment(commentDto));
        }
    }
}
