using Domain.DTOs;

namespace Domain.Entities.Neo4J
{
    public class GameAction
    {
        public int Id { get; set; }
        public int FightId { get; set; }
        public Fight Fight { get; set; }
        public string Type { get; set; }
        public int Value { get; set; }

        public GameAction() { }

    }
}
