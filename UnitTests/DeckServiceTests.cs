using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moq;
using Backend.Services;
using Domain.DTOs;
using Domain.Entities;
using Backend.Repositories.Interfaces;
using Backend.SecurityLogic;  // For Sanitizer
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

            // We won't mock the static Sanitizer, but these tests will call it.
            _deckService = new DeckService(_mockDeckRepository.Object, _mockUserRepository.Object);
        }

        // ---------------------------------------------------------
        // 1. CreateDeck(...)
        // ---------------------------------------------------------
        [Fact]
        public void CreateDeck_ShouldCallRepositoryAddDeck_AndReturnNewDeck()
        {
            // Arrange
            var inputDeck = new DeckDTO
            {
                Id = 0, // ID not yet assigned
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
            Assert.Equal("New Deck", result.Name);
            _mockDeckRepository.Verify(repo => repo.AddDeck(inputDeck), Times.Once);
        }

        // ---------------------------------------------------------
        // 2. DeleteDeck(...)
        // ---------------------------------------------------------
        [Fact]
        public void DeleteDeck_ShouldThrowKeyNotFound_WhenDeckDoesNotExist()
        {
            // Arrange
            int deckId = 999;
            _mockDeckRepository
                .Setup(repo => repo.GetDeckById(deckId))
                .Returns((DeckDTO)null);

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _deckService.DeleteDeck(deckId));
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

        // ---------------------------------------------------------
        // 3. GetDeckById(...)
        // ---------------------------------------------------------
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
            Assert.Equal("Test Deck", result.Name);
        }

        // ---------------------------------------------------------
        // 4. GetUserDecks(...)
        // ---------------------------------------------------------
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
            Assert.Equal("Deck A", result[0].Name);
            Assert.Equal("Deck B", result[1].Name);
        }

        [Fact]
        public void GetUserDecks_ShouldReturnNullIfNoDecksFound()
        {
            // Arrange
            string userName = "nobody";
            _mockDeckRepository
                .Setup(repo => repo.GetUserDecks(userName))
                .Returns((List<DeckDTO>)null);

            // Act
            var result = _deckService.GetUserDecks(userName);

            // Assert
            Assert.Null(result);
        }

        // ---------------------------------------------------------
        // 5. UpdateDeck(...)
        // ---------------------------------------------------------
        [Fact]
        public void UpdateDeck_ShouldCallRepositoryUpdateDeck_AndReturnUpdatedDeck()
        {
            // Arrange
            var deckToUpdate = new DeckDTO
            {
                Id = 100,
                Name = "Updated Name"
            };

            // The repository first updates, then returns the updated deck from GetDeckById
            var updatedDeck = new DeckDTO
            {
                Id = 100,
                Name = "Updated Name",
                IsPublic = true
            };

            _mockDeckRepository
                .Setup(repo => repo.GetDeckById(deckToUpdate.Id))
                .Returns(updatedDeck);

            // Act
            var result = _deckService.UpdateDeck(deckToUpdate);

            // Assert
            _mockDeckRepository.Verify(repo => repo.UpdateDeck(deckToUpdate), Times.Once);
            Assert.Equal("Updated Name", result.Name);
            Assert.True(result.IsPublic);
        }

        // ---------------------------------------------------------
        // 6. GetPublicDecks(...)
        // ---------------------------------------------------------
        [Fact]
        public void GetPublicDecks_ShouldReturnListOfPublicDecks()
        {
            // Arrange
            var publicDecks = new List<DeckDTO>
            {
                new DeckDTO { Id = 1, Name = "Public Deck 1" },
                new DeckDTO { Id = 2, Name = "Public Deck 2" }
            };

            _mockDeckRepository
                .Setup(repo => repo.GetPublicDecks())
                .Returns(publicDecks);

            // Act
            var result = _deckService.GetPublicDecks();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Public Deck 1", result[0].Name);
            Assert.Equal("Public Deck 2", result[1].Name);
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

        // ---------------------------------------------------------
        // 7. AddComment(...)
        //    (We already have tests for exceptions, let's add a "happy path" test)
        // ---------------------------------------------------------
        [Fact]
        public void AddComment_ShouldAddComment_WhenDeckAndUserExist()
        {
            // Arrange
            var comment = new CommentDTO
            {
                DeckId = 10,
                Username = "testUser",
                Text = "Original comment text"
            };

            // Our deck does exist
            _mockDeckRepository
                .Setup(repo => repo.GetDeckById(comment.DeckId))
                .Returns(new DeckDTO { Id = comment.DeckId });

            // Our user does exist
            _mockUserRepository
                .Setup(repo => repo.GetUserByUsername("testUser"))
                .Returns(new UserDTO
                {
                    Id = 42,
                    Username = "testUser",
                    Password = "somePassword",  // required property
                    Role = "someRole"           // required property
                });

            // Act
            _deckService.AddComment(comment);

            // Assert: verify the sanitized comment was added
            _mockDeckRepository.Verify(
                repo => repo.AddComment(It.Is<CommentDTO>(c =>
                    c.DeckId == 10 &&
                    c.UserId == 42 &&
                    // We don't check sanitized text exactly, 
                    // but we confirm it was "some" text
                    !string.IsNullOrWhiteSpace(c.Text)
                )),
                Times.Once
            );
        }

        [Fact]
        public void AddComment_ShouldThrow_KeyNotFound_WhenDeckDoesNotExist()
        {
            // (Already tested in your existing code, but repeated here for completeness)
            var commentDto = new CommentDTO { DeckId = 1, Username = "someUser" };

            _mockDeckRepository
                .Setup(repo => repo.GetDeckById(commentDto.DeckId))
                .Returns((DeckDTO)null);

            Assert.Throws<KeyNotFoundException>(() => _deckService.AddComment(commentDto));
        }

        [Fact]
        public void AddComment_ShouldThrow_KeyNotFound_WhenUserDoesNotExist()
        {
            // (Already tested in your existing code, repeated)
            var commentDto = new CommentDTO { DeckId = 1, Username = "non_existent_user" };

            _mockDeckRepository
                .Setup(repo => repo.GetDeckById(commentDto.DeckId))
                .Returns(new DeckDTO { Id = commentDto.DeckId });

            _mockUserRepository
                .Setup(repo => repo.GetUserByUsername("non_existent_user"))
                .Returns((UserDTO)null);

            Assert.Throws<KeyNotFoundException>(() => _deckService.AddComment(commentDto));
        }

        // ---------------------------------------------------------
        // 8. GetCommentsByDeckId(...)
        //    (We already have partial coverage, let's add coverage for the scenario 
        //     where the deck is found but the comments are null or empty => throws)
        // ---------------------------------------------------------
        [Fact]
        public void GetCommentsByDeckId_ShouldThrowException_IfNoCommentsFound()
        {
            // Arrange
            int deckId = 1;

            _mockDeckRepository
                .Setup(repo => repo.GetDeckById(deckId))
                .Returns(new DeckDTO { Id = deckId, Name = "Some Deck" });

            // Return null or empty list from GetCommentsByDeckId
            _mockDeckRepository
                .Setup(repo => repo.GetCommentsByDeckId(deckId))
                .Returns(new List<CommentDTO>()); // empty

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _deckService.GetCommentsByDeckId(deckId));
            Assert.Contains("No comments found for Deck with ID 1", ex.Message);
        }

        [Fact]
        public void GetCommentsByDeckId_ShouldReturnSanitizedComments_WhenDeckExistsAndCommentsFound()
        {
            // Arrange
            int deckId = 2;
            var deck = new DeckDTO { Id = deckId, Name = "Another Deck" };
            var comments = new List<CommentDTO>
            {
                new CommentDTO { Id = 1, DeckId = deckId, Text = "Some text", Username = "UserA" },
                new CommentDTO { Id = 2, DeckId = deckId, Text = "Other text", Username = "UserB" }
            };

            _mockDeckRepository
                .Setup(repo => repo.GetDeckById(deckId))
                .Returns(deck);
            _mockDeckRepository
                .Setup(repo => repo.GetCommentsByDeckId(deckId))
                .Returns(comments);

            // Act
            var result = _deckService.GetCommentsByDeckId(deckId);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Some text", result[0].Text);
            Assert.Equal("Other text", result[1].Text);
        }

        // ---------------------------------------------------------
        // Existing tests (your original ones) - We keep them
        // They also contribute to coverage
        // ---------------------------------------------------------

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
