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
            var dbCard = _context.Cards.First(x => x.Id == card.Id);

            _context.Cards.Remove(dbCard);
            _context.SaveChanges();
        }

        public List<CardDTO> GetAllCards()
        {
            return _context.Cards.Select(Card.FromEntity).ToList();
        }

        public CardDTO GetCardById(int id)
        {
            var dbCard = _context.Cards.Find(id);
            return Card.FromEntity(dbCard);
        }

        public void UpdateCard(CardDTO card)
        {
            var dbCard = _context.Cards.First(x => x.Id == card.Id);
            dbCard.Cost = card.Cost;
            dbCard.Name = card.Name;
            dbCard.Attack = card.Attack;
            dbCard.Defence = card.Defence;
            dbCard.Description = card.Description;
            dbCard.ImagePath = card.ImagePath;

            _context.Cards.Update(dbCard);
            _context.SaveChanges();
        }

    }
}
