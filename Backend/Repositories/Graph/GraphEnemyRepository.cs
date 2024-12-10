using Infrastructure.Persistance.Relational;
using Backend.Repositories.Interfaces;
using Domain.DTOs;
using Infrastructure.Persistance.Graph;
using Domain.Entities.Neo4J;

namespace Backend.Repositories.Graph
{
    public class GraphEnemyRepository : IEnemyRepository
    {
        private readonly GraphContext _context;

        public GraphEnemyRepository(GraphContext context)
        {
            _context = context;
        }

        public EnemyDTO AddEnemy(EnemyDTO enemy)
        {
            var dbEnemy = Enemy.ToEntity(enemy);
            dbEnemy.Id = _context.GetAutoIncrementedId<Enemy>().Result;
            _context.Insert(dbEnemy).Wait();
            return GetEnemyById(dbEnemy.Id);
        }

        public void DeleteEnemy(EnemyDTO enemy)
        {
            _context.Delete<Enemy>(enemy.Id).Wait();
        }

        public List<EnemyDTO> GetAllEnemies()
        {
            return _context
                .ExecuteQueryWithMap<Enemy>()
                .Result
                .Select(x => Enemy.FromEntity(x))
                .ToList();
        }

        public EnemyDTO GetEnemyById(int id)
        {
            return _context
                .ExecuteQueryWithWhere<Enemy>(x=> x.Id == id)
                .Result
                .Select(x => Enemy.FromEntity(x))
                .FirstOrDefault();
        }

        public void UpdateEnemy(EnemyDTO enemy)
        {
            var dbEnemy = Enemy.ToEntity(enemy);
            _context.UpdateNode(dbEnemy).Wait();
        }
    }
}

