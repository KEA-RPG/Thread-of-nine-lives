using Domain.DTOs;

namespace Domain.Entities
{
    public class Enemy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Health { get; set; }
        public string ImagePath { get; set; }
        public List<Fight> Fights { get; set; }

        public Enemy() { }

        public static Enemy FromDTO(EnemyDTO enemyDTO)
        {
            return new Enemy
            {
                Id = enemyDTO.Id,
                Name = enemyDTO.Name,
                Health = enemyDTO.Health,
                ImagePath = enemyDTO.ImagePath
            };
        }
    }
}
