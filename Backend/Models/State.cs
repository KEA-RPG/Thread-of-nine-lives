using Domain.DTOs;
using Domain.Entities;

namespace Backend.Models
{
    public class State
    {
        
        public List<GameActionDTO> GameActions { get; set; }
        public int PlayerHealth { set;  get; }
        public int EnemyHealth { set;  get; }
        public int FightId { get; set; }

        public State(int playerHealth, int enemyHealth, int fightId)
        {
            FightId = fightId;
            PlayerHealth = playerHealth;
            EnemyHealth = enemyHealth;
        }

        public State() { }
    }
}
