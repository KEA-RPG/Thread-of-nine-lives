using Domain.Entities;
using Domain.DTOs;
using Backend.Repositories.Interfaces;

namespace Backend.Services
{
    public class EnemyService : IEnemyService
    {
        private readonly IEnemyRepository _enemyRepository;

        public EnemyService(IEnemyRepository enemyRepository)
        {
            _enemyRepository = enemyRepository;
        }

        public EnemyDTO GetEnemyById(int id)
        {
            var enemy = _enemyRepository.GetEnemyById(id);
            return enemy;
        }

        public List<EnemyDTO> GetAllEnemies()
        {
            var enemies = _enemyRepository.GetAllEnemies();
            return enemies.ToList();
        }

        public EnemyDTO CreateEnemy(EnemyDTO enemyDTO)
        {
            var enemy = _enemyRepository.AddEnemy(enemyDTO);
            return enemy;
        }

        public EnemyDTO UpdateEnemy(EnemyDTO enemyDTO)
        {
            var existingEnemy = _enemyRepository.GetEnemyById(enemyDTO.Id);
            if (existingEnemy == null)
            {
                return null;
            }

            // Update properties
            existingEnemy.Name = enemyDTO.Name;
            existingEnemy.Health = enemyDTO.Health;
            existingEnemy.ImagePath = enemyDTO.ImagePath;

            _enemyRepository.UpdateEnemy(existingEnemy);

            return _enemyRepository.GetEnemyById(existingEnemy.Id);
        }

        public void DeleteEnemy(int id)
        {
            var enemy = _enemyRepository.GetEnemyById(id);
            if (enemy != null)
            {
                _enemyRepository.DeleteEnemy(enemy);
            }
        }
    }
}
