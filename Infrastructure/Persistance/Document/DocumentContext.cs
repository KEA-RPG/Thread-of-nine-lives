using Domain.DTOs;
using Domain.Entities.Mongo;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Infrastructure.Persistance.Document
{
    public class DocumentContext : DbContext
    {
        private readonly IMongoDatabase _database;
        private readonly MongoClient _client;

        public DocumentContext(string connectionstring, string dbname)
        {
            _client = new MongoClient(connectionstring);
            _database = _client.GetDatabase(dbname);
        }

        public MongoClient GetClient()
        {
            return _client;
        }

        // Add collections as properties

        public IMongoCollection<CardDTO> Cards()
        {
            return GetCollection<CardDTO>("cards");
        }
        public IMongoCollection<DeckDTO> Decks()
        {
            return GetCollection<DeckDTO>("decks");
        }
        public IMongoCollection<UserDTO> Users()
        {
            return GetCollection<UserDTO>("users");
        }
        public IMongoCollection<EnemyDTO> Enemies()
        {
            return GetCollection<EnemyDTO>("enemies");
        }

        public IMongoCollection<FightDTO> Fights()
        {
            return GetCollection<FightDTO>("fights");
        }
        public IMongoCollection<Counter> Counters()
        {
            return GetCollection<Counter>("counters");
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }

        public IAsyncCursor<string> GetAllCollections()
        {
            return _database.ListCollectionNames();
        }

        public int GetAutoIncrementedId(string name)
        {
            var filter = Builders<Counter>.Filter.Eq(x => x.Identifier, name);
            var update = Builders<Counter>.Update.Inc(x => x.Count, 1);

            var options = new FindOneAndUpdateOptions<Counter>
            {
                ReturnDocument = ReturnDocument.After,
                IsUpsert = true
            };

            var updatedCounter = Counters().FindOneAndUpdate(filter, update, options);

            if (updatedCounter == null)
                throw new Exception("Failed to update or retrieve the counter document.");

            return updatedCounter.Count;
        }

    }
}
