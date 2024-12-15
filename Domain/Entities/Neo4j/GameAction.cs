using Domain.DTOs;
using Domain.Entities.Neo4j;

namespace Domain.Entities.Neo4J
{
    public class GameAction : Neo4jBase
    {
        public override int Id { get; set; }
        public int FightId { get; set; }
        public Fight Fight { get; set; }
        public string Type { get; set; }
        public int Value { get; set; }

        public GameAction() { }
        public static GameAction ToEntity(GameActionDTO gameActionDTO)
        {
            return new GameAction
            {
                Id = gameActionDTO.Id,
                FightId = gameActionDTO.FightId,
                Type = gameActionDTO.Type,
                Value = gameActionDTO.Value,
            };
        }
        public static GameActionDTO FromEntity(GameAction gameAction)
        {
            return new GameActionDTO
            {
                Id = gameAction.Id,
                FightId = gameAction.FightId,
                Type = gameAction.Type,
                Value = gameAction.Value,
            };
        }
    }
}
