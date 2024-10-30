using Domain.DTOs;

namespace Domain.Entities
{
    public class Player
    {
        public int Id { get; set; }
        public int Health { get; set; }

        public Player() { }

        public static Player FromDTO(PlayerDTO playerDTO)
        {
            return new Player
            {
                Id = playerDTO.Id,
                Health = playerDTO.Health,
            };
        }
    }
}
