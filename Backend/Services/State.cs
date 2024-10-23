using Domain.Entities;

namespace Backend.Services
{
    public class State
    {
        public Player Player { get; set; }
        public Enemy Enemy { get; set; }
        public List<Action> Actions { get; set; }

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
            
        }

    }

    public class Player
    {
        public int Health { get; set; }
        
    }

    public class Action
    {
        public string Type { get; set; }
        public int Value { get; set; }
    }
}
