using Domain.Entities;
using Infrastructure.Persistance.Relational;
using Domain.DTOs;
using Microsoft.EntityFrameworkCore;

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

        public DeckDTO AddDeck(DeckDTO deck)
        {
            var dbDeck = Deck.FromDTO(deck);
            _context.Decks.Add(dbDeck);
            _context.SaveChanges();

            return GetDeckById(dbDeck.Id);
        }

        public void DeleteDeck(int deckId)
        {
            var dbDeck = _context.Decks.Find(deckId);
            if (dbDeck != null)
            {
                var deckCards = _context.DeckCards.Where(dc => dc.DeckId == dbDeck.Id);
                _context.DeckCards.RemoveRange(deckCards);
                _context.Decks.Remove(dbDeck);
                _context.SaveChanges();
            }
        }

        public DeckDTO GetDeckById(int id)
        {
            var dbDeck = _context.Decks.
                Include(deck => deck.DeckCards).
                ThenInclude(deckCard => deckCard.Card).
                FirstOrDefault(deck => deck.Id == id);

            var deck = DeckDTO.FromEntity(dbDeck);

            return deck;
        }

        public void UpdateDeck(DeckDTO deckToUpdate)
        {
            var dbDeck = _context.Decks.Find(deckToUpdate.Id);

            if (dbDeck != null)
            {
                // Map the properties from the DTO to the entity
                dbDeck.Name = deckToUpdate.Name;
                dbDeck.Id = deckToUpdate.Id;
                dbDeck.DeckCards = deckToUpdate.Cards.Select(card => new DeckCard
                {
                    CardId = card.Id,
                    DeckId = deckToUpdate.Id
                }).ToList();
                dbDeck.IsPublic = deckToUpdate.IsPublic;


                _context.Decks.Update(dbDeck);
                _context.SaveChanges();
            }
        }

        public List<DeckDTO> GetPublicDecks()
        {
            return _context.Decks.Where(deck => deck.IsPublic).Select(deck => DeckDTO.FromEntity(deck)).ToList();
        }

        public List<DeckDTO> GetUserDecks(string userName)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == userName);
            if (user == null) return null;

            return _context.Decks.Where(deck => deck.User == user).Select(deck => DeckDTO.FromEntity(deck)).ToList();
        }
    }
}
