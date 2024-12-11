using Infrastructure.Persistance.Relational;
using Domain.Entities;
using Backend.Repositories.Interfaces;
using Domain.DTOs;

namespace Backend.Repositories.Relational
{
    public class EnemyRepository : IEnemyRepository
    {
        private readonly RelationalContext _context;

        public EnemyRepository(RelationalContext context)
        {
            _context = context;
        }

        public EnemyDTO AddEnemy(EnemyDTO enemy)
        {
            var dbEnemy = Enemy.ToEntity(enemy);
            _context.Enemies.Add(dbEnemy);
            _context.SaveChanges();
            return GetEnemyById(dbEnemy.Id);
        }

        public void DeleteEnemy(EnemyDTO enemy)
        {
            var dbEnemy = _context.Enemies.First(x=> x.Id == enemy.Id);
            _context.Enemies.Remove(dbEnemy);
            _context.SaveChanges();
        }

        public List<EnemyDTO> GetAllEnemies()
        {
            return _context.Enemies.Select(Enemy.FromEntity).ToList();
        }

        public EnemyDTO GetEnemyById(int id)
        {
            var dbEnemy = _context.Enemies.Find(id);
            return Enemy.FromEntity(dbEnemy);
        }

        public void UpdateEnemy(EnemyDTO enemy)
        {
            var enemyDB = _context.Enemies.Find(enemy.Id);
            enemyDB.Health = enemy.Health;
            enemyDB.Name = enemy.Name;
            enemyDB.ImagePath = enemy.ImagePath;
            _context.Enemies.Update(enemyDB);
            _context.SaveChanges();
        }
    }
}

