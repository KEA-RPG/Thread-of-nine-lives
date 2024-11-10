using Domain.DTOs;
using Domain.Entities;

namespace Backend.Services
{
    public class CombatService : ICombatService
    {
        public List<GameAction> GameActions { get; set; }
        public PlayerDTO PlayerDTO { get; }
        public EnemyDTO EnemyDTO { get; }

        public CombatService(PlayerDTO playerDTO, EnemyDTO enemyDTO)
        {
            GameActions = new List<GameAction>();
            PlayerDTO = playerDTO;
            EnemyDTO = enemyDTO;
        }

        public void ProcessAction(GameAction gameAction)
        {
            GameActions.Add(gameAction);
            UpdateGameState(gameAction);
        }

        public void UpdateGameState(GameAction gameAction)
        {
            if (gameAction.Type == "ATTACK")
            {
                EnemyDTO.Health -= gameAction.Value;
                if (EnemyDTO.Health <= 0)
                {
                    EnemyDTO.Health = 0;
                }
            } 
            else if (gameAction.Type == "END_TURN")
            {
                PerformEnemyTurn();
            }
            
        }

        private void PerformEnemyTurn()
        {
            int enemyAttackValue = 5;
            PlayerDTO.Health -= enemyAttackValue;
            if (PlayerDTO.Health < 0)
            {
                PlayerDTO.Health = 0;
            }

        }

    }
    
}
