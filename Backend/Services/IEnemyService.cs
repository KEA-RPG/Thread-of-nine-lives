using Domain.Entities;
namespace Backend.Services

{
    public interface IEnemyService
    {
        Enemy GetEnemyById(int id);
        List<Enemy> GetAllEnemies();
        Enemy CreateEnemy(Enemy enemy);
        Enemy UpdateEnemy(Enemy enemy);
        void DeleteEnemy(int id);
    }
}

