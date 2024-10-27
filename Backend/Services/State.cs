using Domain.DTOs;
using Domain.Entities;

namespace Backend.Services
{
    public class State
    {
        public Player Player { get; set; }
        public List<GameAction> GameActions { get; set; }
        public string CurrentTurn { get; set; } = "PLAYER";
        public EnemyDTO EnemyDTO { get; }

        public State(Player player, EnemyDTO enemyDTO)
        {
            GameActions = new List<GameAction>();
            Player = player;
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
                ToggleTurn();
            }
            
        }

        private void ToggleTurn()
        {
            if (CurrentTurn == "PLAYER")
            {
                CurrentTurn = "ENEMY";
                PerformEnemyTurn();
            }
            else
            {
                CurrentTurn = "PLAYER";
            }
        }

        private void PerformEnemyTurn()
        {
            int enemyAttackValue = 5;
            Player.Health -= enemyAttackValue;
            if (Player.Health < 0)
            {
                Player.Health = 0;
            }

            CurrentTurn = "PLAYER";
        }

    }

    
}
