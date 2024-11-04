using Domain.Entities;
using Infrastructure.Persistance.Relational;
using Domain.DTOs;
using System.Data.Entity;
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
                foreach (var deckCard in deckCards)
                {
                    _context.DeckCards.Remove(deckCard);
                }
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

        public void UpdateDeck(DeckDTO deck)
        {
            _context.Decks.Update(Deck.FromDTO(deck));
            _context.SaveChanges();
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
