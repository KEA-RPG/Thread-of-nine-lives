using Domain.Entities;



namespace Backend.Repositories
{
    public interface IEnemyRepository
    {
        public void AddEnemy(Enemy enemy);
        public void DeleteEnemy(Enemy enemy);
        public void UpdateEnemy(Enemy enemy);
        public List<Enemy> GetAllEnemies();
        public Enemy GetEnemyById(int id);
    }
}
