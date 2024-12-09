using Domain.Entities;

namespace Domain.DTOs
{
    public class GameActionDTO
    {
        public int Id { get; set; }
        public int FightId { get; set; }
        public string Type { get; set; }
        public int Value { get; set; }

    }
}
