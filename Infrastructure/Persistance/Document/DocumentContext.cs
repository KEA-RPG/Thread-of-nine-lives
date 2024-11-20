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


        private IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }
    }
}
