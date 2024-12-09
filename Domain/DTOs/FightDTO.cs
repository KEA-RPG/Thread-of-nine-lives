using Domain.Entities;
using Domain.DTOs;

namespace Domain.DTOs
{
    public class FightDTO
    {
        public int Id { get; set; }
        public EnemyDTO Enemy { get; set; }
        public int EnemyId { get; set; }
        public int UserId { get; set; }
        public List<GameActionDTO> GameActions { get; set; } = new List<GameActionDTO>();


    }
}
