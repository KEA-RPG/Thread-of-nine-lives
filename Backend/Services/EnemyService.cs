using Microsoft.AspNetCore.Cors.Infrastructure;
using Domain.Entities;
using System;
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

        public Enemy CreateEnemy(Enemy enemy)
        {
            _enemyRepository.AddEnemy(enemy);

            return enemy;
        }

        public void DeleteEnemy(int id)
        {
            var enemy = _enemyRepository.GetEnemyById(id);

            _enemyRepository.DeleteEnemy(enemy);
        }

        public List<Enemy> GetAllEnemies()
        {
            return _enemyRepository.GetAllEnemies();
        }

        public Enemy GetEnemyById(int id)
        {
            return _enemyRepository.GetEnemyById(id);
        }

        public Enemy UpdateEnemy(Enemy enemy)
        {
            _enemyRepository.UpdateEnemy(enemy);
            return _enemyRepository.GetEnemyById(enemy.Id);
        }
    }
}

