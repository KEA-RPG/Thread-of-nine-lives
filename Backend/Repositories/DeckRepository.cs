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

        public DeckDTO AddDeck(Deck deck)
        {
            _context.Decks.Add(deck);
            _context.SaveChanges();

            return DeckDTO.FromEntity(deck);
        }

        public void DeleteDeck(Deck deck)
        {
            var dbDeck = _context.Decks.Find(deck.Id);
            if (deck != null)
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

        public Deck GetDeckById(int id, string userName)
        {
            if(userName != null)
            {
                var user = _context.Users.FirstOrDefault(u => u.Username == userName);
                if (user == null) return null;

                return _context.Decks.FirstOrDefault(deck => deck.Id == id && deck.User == user);
            }


            return _context.Decks.Find(id);
        }

        public void UpdateDeck(Deck deck)
        {
            _context.Decks.Update(deck);
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
