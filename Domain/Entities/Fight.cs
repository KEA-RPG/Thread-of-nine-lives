using Domain.DTOs;

namespace Domain.Entities
{
    public class Fight
    {
        public int Id { get; set; }
        public int EnemyId { get; set; }
        public Enemy Enemy { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<GameAction> GameActions { get; set; } = new List<GameAction>();


        public static Fight FromDTO(FightDTO fight)
        {
            return new Fight
            {
                Id = fight.Id,
                EnemyId = fight.EnemyId,
                UserId = fight.UserId
            };
        }
    }
}
