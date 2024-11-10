using Domain.DTOs;

namespace Domain.Entities
{
    public class Fight
    {
        public int Id { get; set; }
        public int EnemyId { get; set; }
        public int UserId { get; set; }

        public Fight() { }

        public static Fight FromDTO(FightDTO fightDTO)
        {
            return new Fight
            {
                Id = fightDTO.Id,
                EnemyId = fightDTO.EnemyId,
                UserId = fightDTO.UserId
            };
        }
    }
}
