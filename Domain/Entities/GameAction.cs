using Domain.DTOs;

namespace Domain.Entities
{
    public class GameAction
    {
        public int Id { get; set; }
        public int FightId { get; set; }
        public string Type { get; set; }
        public int Value { get; set; }
        public int? CardId { get; set; }

        public GameAction() { }

        public static GameAction FromDTO(GameActionDTO gameActionDTO)
        {
            return new GameAction
            {
                Id = gameActionDTO.Id,
                FightId = gameActionDTO.FightId,
                Type = gameActionDTO.Type,
                Value = gameActionDTO.Value,
                CardId = gameActionDTO.CardId,
            };
        }
    }
}
