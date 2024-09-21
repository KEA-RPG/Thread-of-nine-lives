using Backend.Repositories;
using Backend.Services;
using Domain.Entities;
using Moq;
using Xunit;

namespace UnitTests
{
    public class CardUnit
    {
        //Outer Region for positive tests
        #region Positive Tests 

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



        #region Entity





        #endregion

        #region Repository

        #endregion

        #region Service

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
            Assert.Equal(cards, result);
        }

        [Fact]
        public void CreateCard_Should_Create_A_New_Card()
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

            mockCardRepository.Setup(repo => repo.AddCard(card)).Returns(card);

            var cardService = new CardService(mockCardRepository.Object);

            //Act
            var result = cardService.CreateCard(card);

            //Assert
            Assert.Equal(card, result);
        }





        #endregion

        #region Controller

        #endregion

        #endregion


    }
}