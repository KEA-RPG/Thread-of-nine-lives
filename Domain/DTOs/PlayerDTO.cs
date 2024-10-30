using Domain.Entities;

namespace Domain.DTOs
{
    public class PlayerDTO
    {
        public int Id { get; set; }
        public required int Health { get; set; }

        public static PlayerDTO FromEntity(Player player)
        {
            return new PlayerDTO
            {
                Id = player.Id,
                Health = player.Health,
            };
        }
    }
}
