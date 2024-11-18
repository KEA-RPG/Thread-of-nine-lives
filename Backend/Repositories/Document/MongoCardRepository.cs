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
            var id = GetAutoIncrementedId("card");
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
            var filter = Builders<CardDTO>.Filter.Eq(c => c.Id, id);
            return _context.Cards().Find(filter).FirstOrDefault();
        }
        private int GetAutoIncrementedId(string name)
        {
            // Ensure you use `FindOneAndUpdate` for atomicity
            var filter = Builders<Counter>.Filter.Eq(x => x.Identifier, name);
            var update = Builders<Counter>.Update.Inc(x => x.Count, 1);

            var options = new FindOneAndUpdateOptions<Counter>
            {
                ReturnDocument = ReturnDocument.After, 
                IsUpsert = true                       
            };

            var updatedCounter = _context.Counters().FindOneAndUpdate(filter, update, options);

            if (updatedCounter == null)
                throw new Exception("Failed to update or retrieve the counter document.");

            return updatedCounter.Count;
        }
    }
}
