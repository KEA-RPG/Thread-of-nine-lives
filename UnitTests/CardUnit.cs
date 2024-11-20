using Backend.Repositories.Interfaces;
using Backend.Services;
using Domain.DTOs;
using Domain.Entities;
using Moq;
using Xunit;

namespace UnitTests
{
    public class CardUnit
    {

        //CreateCards for testing
        private IEnumerable<Card> GetTestCards(int count)
        {
            var cards = new List<Card>();
            for (int i = 0; i < count; i++)
            {
                cards.Add(new Card
                {
                    Id = i,
                    Name = $"Card {i}",
                    Description = $"Description {i}",
                    ImagePath = $"Image {i}",
                    Cost = i,
                    Attack = i
                });
            }
            return cards;
        }

        [Fact]
        public void CreateCard_Should_Create_A_New_Card()
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

            mockCardRepository.Setup(repo => repo.AddCard(It.IsAny<CardDTO>()));

            var cardService = new CardService(mockCardRepository.Object);

            //Act
            var result = cardService.CreateCard(card);

            //Assert
            Assert.Equal(card, result);
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