using Backend.Repositories.Interfaces;
using Domain.DTOs;
using Domain.Entities;
using Domain.Entities.Mongo;
using Infrastructure.Persistance.Document;
using MongoDB.Driver;

namespace Backend.Repositories.Document
{
    public class MongoCardRepository : ICardRepository
    {
        private readonly DocumentContext _context;

        public MongoCardRepository(DocumentContext context)
        {
            _context = context;
        }

        public CardDTO AddCard(CardDTO card)
        {
            var id = _context.GetAutoIncrementedId("cards");
            card.Id = id;

            _context.Cards().InsertOne(card);
            return GetCardById(id);
        }

        public void DeleteCard(CardDTO card)
        {
            _context.Cards().DeleteOne(x => x.Id == card.Id);
        }

        public void UpdateCard(CardDTO card)
        {
            var filter = Builders<CardDTO>.Filter.Eq(c => c.Id, card.Id);
            var update = Builders<CardDTO>.Update.Set(c => c, card);

            _context.Cards().UpdateOne(filter, update);
        }

        public List<CardDTO> GetAllCards()
        {
            return _context.Cards().Find(Builders<CardDTO>.Filter.Empty).ToList();
        }

        public CardDTO GetCardById(int id)
        {
            return _context.Cards().Find(x => x.Id == id).FirstOrDefault();
        }

    }
}
