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
            if (enemy != null)
            {
                return enemy;
            }
            else
            {
                return null;
            }
        }

        public List<EnemyDTO> GetAllEnemies()
        {
            var enemies = _enemyRepository.GetAllEnemies();
            return enemies.ToList();
        }

        public void CreateEnemy(EnemyDTO enemyDTO)
        {
            _enemyRepository.AddEnemy(enemyDTO);
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
