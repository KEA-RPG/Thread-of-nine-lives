using Domain.DTOs;
using Domain.Entities;

namespace Backend.Models
{
    public class State
    {
        
        public List<GameAction> GameActions { get; set; }
        public int PlayerHealth { set;  get; }
        public int EnemyHealth { set;  get; }

        public State(int playerHealth, int enemyHealth)
        {
            PlayerHealth = playerHealth;
            EnemyHealth = enemyHealth;
        }

        public State() { }
    }
}
