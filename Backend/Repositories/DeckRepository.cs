using Domain.Entities;
using Infrastructure.Persistance.Relational;

namespace Backend.Repositories
{
    public class DeckRepository : IDeckRepository
    {
        private readonly RelationalContext _context;

        public DeckRepository(RelationalContext context)
        {
            _context = context;
        }

        public void AddDeck(Deck deck)
        {
            _context.Decks.Add(deck);
            _context.SaveChanges();
        }

        public void DeleteDeck(Deck deck)
        {
            _context.Decks.Remove(deck);
            _context.SaveChanges();
        }

        public Deck GetDeckById(int id)
        {
            return _context.Decks.Find(id);   
        }

        public void UpdateDeck(Deck deck)
        {
            _context.Decks.Update(deck);
            _context.SaveChanges();
        }

        public List<Deck> GetUserDecks(int id)
        {
            return _context.Decks.Where(d => d.UserId == id).ToList();
        }
    }
}
