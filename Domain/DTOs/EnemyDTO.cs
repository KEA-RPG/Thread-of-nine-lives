using Domain.Entities;

namespace Domain.DTOs
{
    public class EnemyDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required int Health { get; set; }
        public required string ImagePath { get; set; }

    }
}
