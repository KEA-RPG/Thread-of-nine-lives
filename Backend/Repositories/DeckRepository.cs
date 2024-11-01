using Domain.Entities;
using Infrastructure.Persistance.Relational;
using Domain.DTOs;


namespace Backend.Repositories
{
    //Recieves DTO looks for Entities
    //Sends DTO's back
    public class DeckRepository : IDeckRepository
    {
        private readonly RelationalContext _context;

        public DeckRepository(RelationalContext context)
        {
            _context = context;
        }

        public DeckDTO AddDeck(DeckDTO deckDto)
        {
            var deck = new Deck
            {
                Name = deckDto.Name,
                UserId = deckDto.UserId,
                DeckCards = new List<DeckCard>()
            };

            foreach (var card in deckDto.Cards)
            {
                var deckCard = new DeckCard
                {
                    CardId = card.Id,
                    Deck = deck
                };
                deck.DeckCards.Add(deckCard);
            }

            _context.Decks.Add(deck);
            _context.SaveChanges();

            return GetDeckById(deck.Id);
        }

        public IResult DeleteDeck(DeckDTO deckDto)
        {
            var deck = _context.Decks.Find(deckDto.Id);
            if (deck != null)
            {
                var deckCards = _context.DeckCards.Where(dc => dc.DeckId == deck.Id);
                foreach (var deckCard in deckCards)
                {
                    _context.DeckCards.Remove(deckCard);
                }
                _context.Decks.Remove(deck);
                _context.SaveChanges();
                return Results.Ok("Deck Deleted!");
            }
            return Results.NotFound("No Deck with that ID exists");
        }

        public DeckDTO GetDeckById(int id)
        {
            var deck = _context.Decks.Find(id);
            if (deck == null) return null;

            return new DeckDTO
            {
                // Map properties from deck to deckDto
                Id = id,
                UserId = deck.UserId,
                Name = deck.Name,
                IsPublic = deck.IsPublic, // Include the IsPublic property
                Cards = deck.DeckCards.Select(dc => new CardDTO
                {
                    // Map properties from dc.Card to cardDto
                    Id = dc.Card.Id,
                    Name = dc.Card.Name,
                    Description = dc.Card.Description,
                    Attack = dc.Card.Attack,
                    Defense = dc.Card.Defense,
                    Cost = dc.Card.Cost,
                    ImagePath = dc.Card.ImagePath
                }).ToList(),
            };
        }

        public IResult UpdateDeck(DeckDTO deckDto)
        {
            var deck = _context.Decks.Find(deckDto.Id);
            if (deck != null)
            {
                // Map properties from deckDto to deck
                _context.Decks.Update(deck);
                _context.SaveChanges();
                return Results.Ok("Deck updated!");
            }
            return Results.NotFound("No Deck with that ID exists");
        }

        public List<DeckDTO> GetPublicDecks()
        {
            return _context.Decks.Where(deck => deck.IsPublic).Select(deck => new DeckDTO
            {
                // Map properties from deck to deckDto
                Id = deck.Id,
                UserId = deck.UserId,
                Name = deck.Name,
                IsPublic = deck.IsPublic, // Include the IsPublic property
                Cards = deck.DeckCards.Select(dc => new CardDTO
                {
                    // Map properties from dc.Card to cardDto
                    Id = dc.Card.Id,
                    Name = dc.Card.Name,
                    Description = dc.Card.Description,
                    Attack = dc.Card.Attack,
                    Defense = dc.Card.Defense,
                    Cost = dc.Card.Cost,
                    ImagePath = dc.Card.ImagePath
                }).ToList(),
            }).ToList();

        }

        public List<DeckDTO> GetUserDecks(string userName)
        {

            var user = _context.Users.FirstOrDefault(u => u.Username == userName);
            if (user == null) return null;

            return _context.Decks.Where(deck => deck.User == user).Select(deck => new DeckDTO
            {
                // Map properties from deck to deckDto
                Id = deck.Id,
                UserId = deck.UserId,
                Name = deck.Name,
                IsPublic = deck.IsPublic, // Include the IsPublic property
                Cards = deck.DeckCards.Select(dc => new CardDTO
                {
                    // Map properties from dc.Card to cardDto
                    Id = dc.Card.Id,
                    Name = dc.Card.Name,
                    Description = dc.Card.Description,
                    Attack = dc.Card.Attack,
                    Defense = dc.Card.Defense,
                    Cost = dc.Card.Cost,
                    ImagePath = dc.Card.ImagePath
                }).ToList(),
            }).ToList();
        }
    }
}
