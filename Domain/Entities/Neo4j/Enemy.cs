﻿using Domain.DTOs;
using Domain.Entities.Neo4j;

namespace Domain.Entities.Neo4J
{
    public class Enemy : Neo4jBase
    {
        public override int Id { get; set; }
        public string Name { get; set; }
        public int Health { get; set; }
        public string ImagePath { get; set; }
        public List<Fight> Fights { get; set; }

        public Enemy() { }
        public static Enemy ToEntity(EnemyDTO enemyDTO)
        {
            return new Enemy
            {
                Id = enemyDTO.Id,
                Name = enemyDTO.Name,
                Health = enemyDTO.Health,
                ImagePath = enemyDTO.ImagePath
            };
        }
        public static EnemyDTO FromEntity(Enemy enemy)
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
