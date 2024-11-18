using Xunit;
using Moq;
using Backend.Services;
using Domain.Entities;
using Domain.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.Generic;
using System.Linq;
using Backend.Repositories.Interfaces;

namespace Backend.Tests.Services
{
    public class CardServiceTests
    {
        private readonly Mock<ICardRepository> _cardRepositoryMock;
        private readonly CardService _cardService;

        public CardServiceTests()
        {
            _cardRepositoryMock = new Mock<ICardRepository>();
            _cardService = new CardService(_cardRepositoryMock.Object);
        }

        private CardDTO CreateCardDTO()
        {
            return new CardDTO { Name = "Test Card", Attack = 1, Cost = 1, Defence = 1, Description = "description", ImagePath = "image.png" };
        }

        [Fact]
        public void CreateCard_ShouldReturnCardDTO_WithIdAssigned()
        {
            // Arrange
            var cardDTO = CreateCardDTO();
            var card = new Card { Id = 1, Name = "Test Card" };

            _cardRepositoryMock.Setup(r => r.AddCard(It.IsAny<Card>()))
                               .Callback<Card>(c => c.Id = card.Id); // Simulate ID assignment

            // Act
            var result = _cardService.CreateCard(cardDTO);

            // Assert
            Assert.Equal(card.Id, result.Id);
            _cardRepositoryMock.Verify(r => r.AddCard(It.Is<Card>(c => c.Name == cardDTO.Name)), Times.Once);
        }

/*        [Fact]
        public void DeleteCard_ShouldReturnBadRequest_WhenCardNotFound()
        {
            // Arrange
            int id = 1;
            _cardRepositoryMock.Setup(r => r.GetCardById(id)).Returns((Card)null);

            // Act
            var result = _cardService.DeleteCard(id);

            // Assert
            Assert.IsType<BadRequest<string>>(result);
        }*/
/*
        [Fact]
        public void DeleteCard_ShouldReturnOkResult_WhenCardIsDeleted()
        {
            // Arrange
            var card = new Card { Id = 1, Name = "Test Card" };
            _cardRepositoryMock.Setup(r => r.GetCardById(card.Id)).Returns(card);

            // Act
            var result = _cardService.DeleteCard(card.Id);

            // Assert
            Assert.IsType<Ok<string>>(result);
            _cardRepositoryMock.Verify(r => r.DeleteCard(card), Times.Once);
        }*/

        [Fact]
        public void GetAllCards_ShouldReturnListOfCardDTOs()
        {
            // Arrange
            var cards = new List<Card> { new Card { Id = 1, Name = "Card 1" }, new Card { Id = 2, Name = "Card 2" } };
            _cardRepositoryMock.Setup(r => r.GetAllCards()).Returns(cards);

            // Act
            var result = _cardService.GetAllCards();

            // Assert
            Assert.Equal(cards.Count, result.Count);
            Assert.Equal(cards.First().Id, result.First().Id);
        }

        [Fact]
        public void GetCardById_ShouldReturnCardDTO_WhenCardExists()
        {
            // Arrange
            var card = new Card { Id = 1, Name = "Test Card" };
            _cardRepositoryMock.Setup(r => r.GetCardById(card.Id)).Returns(card);

            // Act
            var result = _cardService.GetCardById(card.Id);

            // Assert
            Assert.Equal(card.Id, result.Id);
            Assert.Equal(card.Name, result.Name);
        }

        [Fact]
        public void GetCardById_ShouldThrowKeyNotFoundException_WhenCardNotFound()
        {
            // Arrange
            int id = 1;
            _cardRepositoryMock.Setup(r => r.GetCardById(id)).Returns((Card)null);

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _cardService.GetCardById(id));
        }

        [Fact]
        public void UpdateCard_ShouldReturnUpdatedCardDTO_WhenCardExists()
        {
            // Arrange
            var cardDTO = CreateCardDTO();
            var existingCard = new Card { Id = cardDTO.Id, Name = "Old Card", Description = "Old Description" };

            _cardRepositoryMock.Setup(r => r.GetCardById(cardDTO.Id)).Returns(existingCard);

            // Act
            var result = _cardService.UpdateCard(cardDTO);

            // Assert
            Assert.Equal(cardDTO.Name, result.Name);
            Assert.Equal(cardDTO.Description, result.Description);
            _cardRepositoryMock.Verify(r => r.UpdateCard(existingCard), Times.Once);
        }

        [Fact]
        public void UpdateCard_ShouldThrowKeyNotFoundException_WhenCardDoesNotExist()
        {
            // Arrange
            var cardDTO = CreateCardDTO();
            _cardRepositoryMock.Setup(r => r.GetCardById(cardDTO.Id)).Returns((Card)null);

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _cardService.UpdateCard(cardDTO));
        }
    }
}
