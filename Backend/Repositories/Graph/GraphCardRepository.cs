using Backend.Repositories.Interfaces;
using Domain.DTOs;
using Domain.Entities.Neo4J;
using Infrastructure.Persistance.Graph;

namespace Backend.Repositories.Graph
{
    public class GraphCardRepository : ICardRepository
    {
        private readonly GraphContext _context;

        public GraphCardRepository(GraphContext context)
        {
            _context = context;
        }

        public CardDTO AddCard(CardDTO cardDto)
        {
            var card = Card.ToEntity(cardDto);
            card.Id = _context.GetAutoIncrementedId<Card>().Result;
            _context.Insert(card).Wait();
            return GetCardById(card.Id);
        }

        public void DeleteCard(CardDTO card)
        {
            _context.Delete<Card>(card.Id).Wait();
        }

        public List<CardDTO> GetAllCards()
        {
            return _context
                .ExecuteQueryWithMap<Card>()
                .Result
                .Select(x=> Card.FromEntity(x))
                .ToList();
        }

        public CardDTO GetCardById(int id)
        {
            return _context
                .ExecuteQueryWithMap<Card>()
                .Result
                .Select(x => Card.FromEntity(x))
                .FirstOrDefault();
        }

        public void UpdateCard(CardDTO card)
        {
            var dbCard = Card.ToEntity(card);
            _context.UpdateNode(dbCard).Wait();
        }

    }
}
