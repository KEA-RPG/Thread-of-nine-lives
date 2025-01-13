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
using MongoDB.Driver;

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
            return new CardDTO
            {
                Name = "Test Card",
                Attack = 1,
                Cost = 1,
                Defence = 1,
                Description = "description",
                ImagePath = "image.png"
            };
        }

        [Fact]
        public void CreateCard_ShouldReturnCardDTO_WithIdAssigned()
        {
            // Arrange
            var cardDTO = CreateCardDTO();
            var returnCardDTO = CreateCardDTO();
            returnCardDTO.Id = 1;
            _cardRepositoryMock.Setup(r => r.AddCard(It.IsAny<CardDTO>()))
                               .Returns(returnCardDTO); // Simulate ID assignment

            // Act
            var result = _cardService.CreateCard(cardDTO);

            // Assert
            Assert.NotEqual(result.Id, 0);

            _cardRepositoryMock.Verify(r => r.AddCard(It.Is<CardDTO>(c => c.Name == cardDTO.Name)), Times.Once);
        }

        [Fact]
        public void GetAllCards_ShouldReturnListOfCardDTOs()
        {
            // Arrange
            var cards = new List<CardDTO> {
                new CardDTO { Id = 1,
                    Name = "Card 1", Attack= 1, Cost = 1,Defence = 1, Description = "description", ImagePath = "test"
                }, new CardDTO { Id = 2,
                    Name = "Card 2", Attack= 1, Cost = 1,Defence = 1, Description = "description", ImagePath = "test"
                }
            };
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
            var card = new CardDTO { Id = 1, Name = "Test Card", Attack = 1, Cost = 1, Defence = 1, Description = "description", ImagePath = "test" };
            _cardRepositoryMock.Setup(r => r.GetCardById(card.Id)).Returns(card);

            // Act
            var result = _cardService.GetCardById(card.Id);

            // Assert
            Assert.Equal(card.Id, result.Id);
            Assert.Equal(card.Name, result.Name);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(999)]
        public void GetCardById_ShouldThrowKeyNotFoundException_WhenCardNotFound(int id)
        {
            // Arrange
            _cardRepositoryMock.Setup(r => r.GetCardById(id)).Returns((CardDTO)null);

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _cardService.GetCardById(id));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(5)]
        public void GetAllCards_ReturnsCorrectNumberOfCards(int numberOfEnemies)
        {
            // Arrange
            var cards = Enumerable.Range(1, numberOfEnemies)
                .Select(id => new CardDTO
                {
                    Id = id,
                    Name = $"Card{id}",
                    ImagePath = $"Card{id}",
                    Description = $"This is descriptions of Card{id}",
                    Defence = id,
                    Cost = id,
                    Attack = id,
                })
                .ToList();
            _cardRepositoryMock.Setup(repo => repo.GetAllCards()).Returns(cards);

            // Act
            var result = _cardService.GetAllCards();

            // Assert
            Assert.Equal(numberOfEnemies, result.Count);
        }

        [Fact]
        public void UpdateCard_ShouldReturnUpdatedCardDTO_WhenCardExists()
        {
            // Arrange
            var cardDTO = CreateCardDTO();
            var existingCard = new CardDTO
            {
                Id = cardDTO.Id,
                Name = "Old Card",
                Description = "Old Description",
                Attack = 1,
                Cost = 1,
                Defence = 1,
                ImagePath = "test"
            };

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
            _cardRepositoryMock.Setup(r => r.GetCardById(cardDTO.Id)).Returns((CardDTO)null);

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _cardService.UpdateCard(cardDTO));
        }

        [Fact]
        public void DeleteCard_Should_Delete_A_Card()
        {
            //Arrange
            var mockCardRepository = new Mock<ICardRepository>();
            var card = new CardDTO
            {
                Id = 1,
                Name = "Card 1",
                Description = "Description 1",
                ImagePath = "Image 1",
                Cost = 1,
                Attack = 1,
                Defence = 1

            };
            mockCardRepository.Setup(repo => repo.AddCard(card));
            mockCardRepository.Setup(repo => repo.GetCardById(1)).Returns(card);

            var cardService = new CardService(mockCardRepository.Object);

            //Act
            cardService.DeleteCard(1);

            //Assert
            mockCardRepository.Verify(repo => repo.DeleteCard(card), Times.Once);
        }



    }
}
