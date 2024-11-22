using Backend.Repositories.Interfaces;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Persistance.Document;
using MongoDB.Driver;

namespace Backend.Repositories.Document
{
    public class MongoEnemyRepository : IEnemyRepository
    {
        private readonly DocumentContext _context;

        public MongoEnemyRepository(DocumentContext context)
        {
            _context = context;
        }

        public EnemyDTO AddEnemy(EnemyDTO enemy)
        {
            var id = _context.GetAutoIncrementedId("enemies");
            enemy.Id = id;

            _context.Enemies().InsertOne(enemy);
            return GetEnemyById(id);
        }

        public void DeleteEnemy(EnemyDTO enemy)
        {
            _context.Enemies().DeleteOne(x => x.Id == enemy.Id);
        }

        public List<EnemyDTO> GetAllEnemies()
        {
            return _context.Enemies().Find(Builders<EnemyDTO>.Filter.Empty).ToList();
        }

        public EnemyDTO GetEnemyById(int id)
        {
            return _context.Enemies().Find(x => x.Id == id).FirstOrDefault();
        }

        public void UpdateEnemy(EnemyDTO enemy)
        {
            var filter = Builders<EnemyDTO>.Filter.Eq(c => c.Id, enemy.Id);
            var update = Builders<EnemyDTO>.Update.Set(c => c, enemy);

            _context.Enemies().UpdateOne(filter, update);
        }
    }
}
