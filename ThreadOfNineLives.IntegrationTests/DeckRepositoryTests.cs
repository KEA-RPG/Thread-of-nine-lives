using Backend.Repositories.Relational;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Persistance.Relational;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace ThreadOfNineLives.IntegrationTests
{
    public class DeckRepositoryTests
    {
        private readonly RelationalContext _context;

        private readonly DeckRepository _deckRepository;
        private readonly DbContextOptionsBuilder<RelationalContext> _options;


        public DeckRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<RelationalContext>()
            .UseSqlite("DataSource=:memory:");
            _context = new RelationalContext(_options.Options);
            _context.Database.OpenConnection();
            _context.Database.Migrate();

            _deckRepository = new DeckRepository(_context);
        }

        private CardDTO CreateCardDTO()
        {
            return new CardDTO { Name = "Test Card", Attack = 1, Cost = 1, Defence = 1, Description = "description", ImagePath = "image.png" };
        }

        //[Fact]
/*        public void AddDeck_ShouldAddDeckToDatabase()
        {
            // Arrange
            var deckDto = new DeckDTO
            {
                Name = "Test Deck",
                UserId = 1,
                Cards = new List<CardDTO> // Specify the type as CardDTO
                        {
                            CreateCardDTO(),
                            CreateCardDTO()
                        }
            };

            // Act
            var deck = _deckRepository.AddDeck(deckDto);

            // Assert
            Assert.NotNull(deck);
            Assert.Equal(deckDto.Name, deck.Name);
            Assert.Equal(deckDto.UserId, deck.UserId);
            Assert.Equal(deckDto.Cards.Count, deck.Cards.Count);
        }*/
    }
}
