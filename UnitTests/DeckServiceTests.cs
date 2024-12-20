﻿using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using Backend.Services;
using Domain.DTOs;
using Domain.Entities;
using Backend.Repositories.Interfaces;

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
        public void GetCommentsByDeckId_ShouldReturnCommentsForDeck()
        {
            // Arrange
            var deckId = 1;
            var comments = new List<CommentDTO>
            {
                new CommentDTO { Id = 1, DeckId = deckId, Text = "Awesome deck!", CreatedAt = DateTime.UtcNow, Username = "user1" },
                new CommentDTO { Id = 2, DeckId = deckId, Text = "Needs more work!", CreatedAt = DateTime.UtcNow, Username = "user2" }
            };

            _mockDeckRepository.Setup(repo => repo.GetDeckById(deckId)).Returns(new DeckDTO { Id = deckId });
            _mockDeckRepository.Setup(repo => repo.GetCommentsByDeckId(deckId)).Returns(comments);

            // Act
            var result = _deckService.GetCommentsByDeckId(deckId);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Awesome deck!", result[0].Text);
            Assert.Equal("user1", result[0].Username);
            Assert.Equal("Needs more work!", result[1].Text);
            Assert.Equal("user2", result[1].Username);
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

        [Fact]
        public void GetCommentsByDeckId_ShouldThrowException_WhenDeckDoesNotExist()
        {
            // Arrange
            var deckId = 1;

            _mockDeckRepository.Setup(repo => repo.GetDeckById(deckId)).Returns((DeckDTO)null);

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _deckService.GetCommentsByDeckId(deckId));
        }
    }
}
