using System;
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
        private readonly DeckService _deckService;

        public DeckServiceTests()
        {
            _mockDeckRepository = new Mock<IDeckRepository>();
            _deckService = new DeckService(_mockDeckRepository.Object);
        }

        [Fact]
        public void AddComment_ShouldAddCommentToDeck()
        {
            // Arrange
            var deckId = 1;
            var commentDto = new CommentDTO
            {
                Id = 1,
                DeckId = deckId,
                Text = "Great deck!",
                CreatedAt = DateTime.UtcNow
            };

            _mockDeckRepository.Setup(repo => repo.GetDeckById(deckId)).Returns(new DeckDTO { Id = deckId });

            // Act
            _deckService.AddComment(commentDto);

            // Assert
            _mockDeckRepository.Verify(repo => repo.AddComment(It.Is<CommentDTO>(c => c.Text == commentDto.Text && c.DeckId == deckId)), Times.Once);
        }

        [Fact]
        public void GetCommentsByDeckId_ShouldReturnCommentsForDeck()
        {
            // Arrange
            var deckId = 1;
            var comments = new List<CommentDTO>
            {
                new CommentDTO { Id = 1, DeckId = deckId, Text = "Awesome deck!", CreatedAt = DateTime.UtcNow },
                new CommentDTO { Id = 2, DeckId = deckId, Text = "Needs more work!", CreatedAt = DateTime.UtcNow }
            };

            _mockDeckRepository.Setup(repo => repo.GetDeckById(deckId)).Returns(new DeckDTO { Id = deckId });
            _mockDeckRepository.Setup(repo => repo.GetCommentsByDeckId(deckId)).Returns(comments);

            // Act
            var result = _deckService.GetCommentsByDeckId(deckId);

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetCommentsByDeckId_ShouldReturnCorrectCommentText()
        {
            // Arrange
            var deckId = 1;
            var comments = new List<CommentDTO>
            {
                new CommentDTO { Id = 1, DeckId = deckId, Text = "Awesome deck!", CreatedAt = DateTime.UtcNow },
                new CommentDTO { Id = 2, DeckId = deckId, Text = "Needs more work!", CreatedAt = DateTime.UtcNow }
            };

            _mockDeckRepository.Setup(repo => repo.GetDeckById(deckId)).Returns(new DeckDTO { Id = deckId });
            _mockDeckRepository.Setup(repo => repo.GetCommentsByDeckId(deckId)).Returns(comments);

            // Act
            var result = _deckService.GetCommentsByDeckId(deckId);

            // Assert
            Assert.Equal("Awesome deck!", result[0].Text);
            Assert.Equal("Needs more work!", result[1].Text);
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
                CreatedAt = DateTime.UtcNow
            };

            _mockDeckRepository.Setup(repo => repo.GetDeckById(deckId)).Returns((DeckDTO)null);

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
