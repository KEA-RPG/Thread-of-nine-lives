using Domain.DTOs;
using Domain.Entities.Neo4j;

namespace Domain.Entities.Neo4J
{
    public class Fight : Neo4jBase
    {
        public int Id { get; set; }
        public int EnemyId { get; set; }
        public Enemy Enemy { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<GameAction> GameActions { get; set; }
        public static Fight ToEntity(FightDTO fight)
        {
            return new Fight
            {
                Id = fight.Id,
                EnemyId = fight.EnemyId,
                UserId = fight.UserId
            };
        }
        public static FightDTO FromEntity(Fight fight)
        {
            return new FightDTO
            {
                Id = fight.Id,
                Enemy = Enemy.FromEntity(fight.Enemy),
                GameActions = fight.GameActions.Select(x => GameAction.FromEntity(x)).ToList(),
                EnemyId = fight.EnemyId,
                UserId = fight.UserId
            };
        }
    }
}
