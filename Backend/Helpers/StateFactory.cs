using Backend.Models;
using Domain.DTOs;

namespace Backend.Helpers
{
    public class StateFactory
    {
        public State CreateState(FightDTO fight)
        {
            int playerHealth = 25;
            int enemyHealth = 50;
            if (fight.Enemy != null)
            {
                enemyHealth = fight.Enemy.Health;
            }
            State state = new State
            {
                FightId = fight.Id,
                GameActions = fight.GameActions,
                PlayerHealth = playerHealth,
                EnemyHealth = enemyHealth
            };

            state = ProcessAction(state, fight.GameActions);

            return state;
        }

        private State ProcessAction(State state, List<GameActionDTO> actions)
        {
            if (state.GameActions != null)
            {
                foreach (var action in actions)
                {
                    if (action.Type == "ATTACK")
                    {
                        state = PerformPlayerAttack(state, action);
                    }
                    else if (action.Type == "END_TURN")
                    {
                        state = PerformEnemyTurn(state, action);
                    }
                }
            }

            return state;
        }

        private State PerformPlayerAttack(State state, GameActionDTO action)
        {
            state.EnemyHealth -= action.Value;
            if (state.EnemyHealth <= 0)
            {
                state.EnemyHealth = 0;
            }

            return state;
        }

        private State PerformEnemyTurn(State state, GameActionDTO action)
        {
            state.PlayerHealth -= 5;
            if (state.PlayerHealth < 0)
            {
                state.PlayerHealth = 0;
            }
            return state;
        }
    }
}
