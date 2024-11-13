
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Persistance.Relational;

namespace Backend.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly RelationalContext _context;

        public CardRepository(RelationalContext context)
        {
            _context = context;
        }

        public void AddCard(Card card)
        {
            _context.Cards.Add(card);
            _context.SaveChanges();
        }

        public void DeleteCard(Card card)
        {
            _context.Cards.Remove(card);
            _context.SaveChanges();
        }

        public List<Card> GetAllCards()
        {
            return _context.Cards.ToList();
        }

        public Card GetCardById(int id)
        {
            return _context.Cards.Find(id);
        }

        public void UpdateCard(Card card)
        {
            _context.Cards.Update(card);
            _context.SaveChanges();
        }

    }
}
