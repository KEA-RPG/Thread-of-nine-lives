using Domain.Entities;

namespace Backend.Models
{
    public class State
    {
        public List<GameAction> GameActions { get; set; }
        public int PlayerHealth { get; }
        public int EnemyHealth { get; }
    }
}
