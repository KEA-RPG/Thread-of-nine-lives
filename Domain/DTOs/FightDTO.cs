using Domain.Entities;
using Domain.DTOs;

namespace Domain.DTOs
{
    public class FightDTO
    {
        public int Id { get; set; }
        public required EnemyDTO Enemy { get; set; }
        public required int UserId { get; set; }

        public static FightDTO FromEntity(Fight fight)
        {
            return new FightDTO
            {
                Id = fight.Id,
                Enemy = EnemyDTO.FromEntity(fight.Enemy),
                UserId = fight.UserId
            };
        }
    }
}
