using Backend.Repositories.Interfaces;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Persistance.Relational;

namespace Backend.Repositories.Relational
{
    public class CardRepository : ICardRepository
    {
        private readonly RelationalContext _context;

        public CardRepository(RelationalContext context)
        {
            _context = context;
        }

        public CardDTO AddCard(CardDTO card)
        {
            var dbCard = Card.ToEntity(card);
            _context.Cards.Add(dbCard);
            _context.SaveChanges();
            return GetCardById(dbCard.Id);
        }

        public void DeleteCard(CardDTO card)
        {
            var dbCard = Card.ToEntity(card);

            _context.Cards.Remove(dbCard);
            _context.SaveChanges();
        }

        public List<CardDTO> GetAllCards()
        {
            return _context.Cards.Select(CardDTO.FromEntity).ToList();
        }

        public CardDTO GetCardById(int id)
        {
            var dbCard = _context.Cards.Find(id);
            return CardDTO.FromEntity(dbCard);
        }

        public void UpdateCard(CardDTO card)
        {
            var dbCard = Card.ToEntity(card);

            _context.Cards.Update(dbCard);
            _context.SaveChanges();
        }

    }
}
