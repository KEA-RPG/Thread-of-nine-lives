
namespace Backend.Services
{
    public class State
    {
        public Player Player { get; set; }
        public Boss Boss { get; set; }
        public List<Action> Actions { get; set; }

        public State(Player player, Boss boss)
        {
            Actions = new List<Action>();
            Player = player;
            Boss = boss;
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
                Boss.Health -= action.Value;
                if (Boss.Health <= 0)
                {
                    Boss.Health = 0;
                }
            }
            
        }

    }

    public class Player
    {
        public int Health { get; set; }
        
    }

    public class Boss
    {
        public int Health { get; set; }
    }

    public class Action
    {
        public string Type { get; set; }
        public int Value { get; set; }
    }
}
