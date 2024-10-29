using Backend.Repositories;
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

         //[Fact]
        public void AddDeck_ShouldAddDeckToDatabase()
        {
            // Arrange
            var deckDto = new DeckDTO
            {
                Name = "Test Deck",
                UserId = 1,
                Cards = new List<CardDTO> // Specify the type as CardDTO
                        {
                            new CardDTO { Id = 1, Name = "Card 1" },
                            new CardDTO { Id = 2, Name = "Card 2" }
                        }
            };

            // Act
            var deck = _deckRepository.AddDeck(deckDto);

            // Assert
            Assert.NotNull(deck);
            Assert.Equal(deckDto.Name, deck.Name);
            Assert.Equal(deckDto.UserId, deck.UserId);
            Assert.Equal(deckDto.Cards.Count, deck.Cards.Count);
        }
    }
}
