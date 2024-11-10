using Domain.Entities;

namespace Domain.DTOs
{
    public class FightDTO
    {
        public int Id { get; set; }
        public required int EnemyId { get; set; }
        public required int UserId { get; set; }

        public static FightDTO FromEntity(Fight fight)
        {
            return new FightDTO
            {
                Id = fight.Id,
                EnemyId = fight.EnemyId,
                UserId = fight.UserId
            };
        }
    }
}
