using Domain.DTOs;
using Domain.Entities;



namespace Backend.Repositories.Interfaces
{
    public interface IEnemyRepository
    {
        public EnemyDTO AddEnemy(EnemyDTO enemy);
        public void DeleteEnemy(EnemyDTO enemy);
        public void UpdateEnemy(EnemyDTO enemy);
        public List<EnemyDTO> GetAllEnemies();
        public EnemyDTO GetEnemyById(int id);
    }
}
