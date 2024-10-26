using Domain.Entities;
using Domain.DTOs;
using Backend.Repositories;

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
                return ToEnemyDTO(enemy);
            }
            else
            {
                return null;
            }
        }

        public List<EnemyDTO> GetAllEnemies()
        {
            var enemies = _enemyRepository.GetAllEnemies();
            return enemies.Select(ToEnemyDTO).ToList();
        }

        public EnemyDTO CreateEnemy(EnemyDTO enemyDTO)
        {
            var enemy = ToEnemy(enemyDTO);
            _enemyRepository.AddEnemy(enemy);

            // Update the DTO with the generated Id from the entity
            enemyDTO.Id = enemy.Id;

            return enemyDTO;
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

            return ToEnemyDTO(existingEnemy);
        }

        public void DeleteEnemy(int id)
        {
            var enemy = _enemyRepository.GetEnemyById(id);
            if (enemy != null)
            {
                _enemyRepository.DeleteEnemy(enemy);
            }
        }

        // Mapping methods
        private Enemy ToEnemy(EnemyDTO enemyDTO)
        {
            return new Enemy
            {
                Id = enemyDTO.Id,
                Name = enemyDTO.Name,
                Health = enemyDTO.Health,
                ImagePath = enemyDTO.ImagePath
            };
        }

        private EnemyDTO ToEnemyDTO(Enemy enemy)
        {
            return new EnemyDTO
            {
                Id = enemy.Id,
                Name = enemy.Name,
                Health = enemy.Health,
                ImagePath = enemy.ImagePath
            };
        }
    }
}
