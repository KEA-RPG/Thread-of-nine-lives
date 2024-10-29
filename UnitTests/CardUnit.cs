using Backend.Repositories;
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

/*
        [Fact]
        public void GetCardById_Should_Return_A_Single_Card()
        {
            //Arrange
            var mockCardRepository = new Mock<ICardRepository>();
            var card = new Card
            {
                Id = 1,
                Name = "Card 1",
                Description = "Description 1",
                ImagePath = "Image 1",
                Cost = 1,
                Attack = 1
            };

            //Mocking the GetCardById method
            mockCardRepository.Setup(repo => repo.GetCardById(1)).Returns(card);

            var cardService = new CardService(mockCardRepository.Object);

            //Act

            var result = cardService.GetCardById(1);

            //Assert
            Assert.Equal(card, result);
            mockCardRepository.Verify(repo => repo.GetCardById(1), Times.Once);


        }

       [Theory]
        [InlineData(1)] //If there should only be 1 card
        [InlineData(3)] //If there should be more than 1 cards
        public void GetAllCards_Should_Return_All_Cards(int count)
        {
            //Arrange
            var mockCardRepository = new Mock<ICardRepository>();
            var cards = GetTestCards(count);

            mockCardRepository.Setup(repo => repo.GetAllCards()).Returns(cards.ToList());

            var cardService = new CardService(mockCardRepository.Object);

            //Act
            var result = cardService.GetAllCards();

            //Assert
            Assert.Equal(cards.Select(c => new CardDTO
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Attack = c.Attack,
                Defense = c.Defense,
                Cost = c.Cost,
                ImagePath = c.ImagePath
            }).ToList(), result);
            //Arrange
            var mockCardRepository = new Mock<ICardRepository>();
            var cards = GetTestCards(count);

            mockCardRepository.Setup(repo => repo.GetAllCards()).Returns(cards.ToList());

            var cardService = new CardService(mockCardRepository.Object);

            //Act
            var result = cardService.GetAllCards();

            //Assert
            Assert.Equal(cards.ToList(), result);
        }*/

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
                Defense = 1
            };

            mockCardRepository.Setup(repo => repo.AddCard(It.IsAny<Card>()));

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
            var card = new Card
            {
                Id = 1,
                Name = "Card 1",
                Description = "Description 1",
                ImagePath = "Image 1",
                Cost = 1,
                Attack = 1

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