using Domain.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistance.Document
{
    public class DocumentContext : DbContext
    {
        private readonly IMongoDatabase _database;

        public DocumentContext(string connectionstring, string dbname)
        {
            var client = new MongoClient(connectionstring);
            _database = client.GetDatabase(dbname);
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

        private IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }
    }
}
