using Infrastructure.Persistance.Relational;
using Domain.Entities;
using Backend.Repositories.Interfaces;

namespace Backend.Repositories.Relational
{
    public class EnemyRepository : IEnemyRepository
    {
        private readonly RelationalContext _context;

        public EnemyRepository(RelationalContext context)
        {
            _context = context;
        }

        public void AddEnemy(Enemy enemy)
        {
            _context.Enemies.Add(enemy);
            _context.SaveChanges();
        }

        public void DeleteEnemy(Enemy enemy)
        {
            _context.Enemies.Remove(enemy);
            _context.SaveChanges();
        }

        public List<Enemy> GetAllEnemies()
        {
            return _context.Enemies.ToList();
        }

        public Enemy GetEnemyById(int id)
        {
            return _context.Enemies.Find(id);
        }

        public void UpdateEnemy(Enemy enemy)
        {
            _context.Enemies.Update(enemy);
            _context.SaveChanges();
        }
    }
}

