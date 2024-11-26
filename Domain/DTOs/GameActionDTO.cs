using Domain.Entities;

namespace Domain.DTOs
{
    public class GameActionDTO
    {
        public int Id { get; set; }
        public int FightId { get; set; }
        public string Type { get; set; }
        public int Value { get; set; }

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
