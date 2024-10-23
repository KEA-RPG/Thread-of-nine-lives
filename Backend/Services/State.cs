using Domain.Entities;

namespace Backend.Services
{
    public class State
    {
        public Player Player { get; set; }
        public Enemy Enemy { get; set; }
        public List<Action> Actions { get; set; }
        string CurrentTurn { get; set; } = "PLAYER";

        public State(Player player, Enemy enemy)
        {
            Actions = new List<Action>();
            Player = player;
            Enemy = enemy;
        }
        public void ProcessAction(Action action)
        {
            Actions.Add(action);
            UpdateGameState(action);
        }

        public void UpdateGameState(Action action)
        {
            if (action.Type == "ATTACK")
            {
                Enemy.Health -= action.Value;
                if (Enemy.Health <= 0)
                {
                    Enemy.Health = 0;
                }
            } 
            else if (action.Type == "END_TURN")
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

    public class Action
    {
        public string Type { get; set; }
        public int Value { get; set; }
    }
}
