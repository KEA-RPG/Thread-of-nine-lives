using Domain.DTOs;

namespace Backend.Services
{
    public interface IEnemyService
    {
        EnemyDTO GetEnemyById(int id);
        List<EnemyDTO> GetAllEnemies();
        EnemyDTO CreateEnemy(EnemyDTO enemyDTO);
        EnemyDTO UpdateEnemy(EnemyDTO enemyDTO);
        void DeleteEnemy(int id);
    }
}
