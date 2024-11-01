using Domain.DTOs;

namespace Domain.Entities
{
    public class GameAction
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int Value { get; set; }

        public GameAction() { }

        public static GameAction FromDTO(GameActionDTO gameActionDTO)
        {
            return new GameAction
            {
                Id = gameActionDTO.Id,
                Type = gameActionDTO.Type,
                Value = gameActionDTO.Value,
            };
        }
    }
}
