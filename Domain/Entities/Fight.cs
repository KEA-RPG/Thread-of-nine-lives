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
        public List<GameAction> GameActions { get; set; }

        public Fight() { }

        public static Fight FromDTO(FightDTO fight)
        {
            return new Fight
            {
                Id = fight.Id,
                GameActions = fight.GameActions.Select(GameAction.FromDTO).ToList(),
                EnemyId = fight.EnemyId,
                UserId = fight.UserId
            };
        }
    }
}
