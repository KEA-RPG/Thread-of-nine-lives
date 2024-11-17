using Backend.Models;
using Domain.DTOs;

namespace Backend.Helpers
{
    public class StateFactory
    {
        public State CreateInitState(FightDTO fight)
        {
            int playerHealth = 25;
            int enemyHealth = 50;
            State state = new State
            {
                FightId = fight.Id,
                GameActions = fight.GameActions,
                PlayerHealth = playerHealth,
                EnemyHealth = enemyHealth
            };

            return state;
        }

        public State ProcessAction (State state)
        {
            if (state.GameActions != null)
            {
                foreach (var gameAction in state.GameActions)
                {
                    if (gameAction.Type == "ATTACK")
                    {
                        state = PerformPlayerAttack(state);
                    }
                    else if (gameAction.Type == "END_TURN")
                    {
                        state = PerformEnemyTurn(state);
                    }
                }
            }

            return state;
        }

        private State PerformPlayerAttack(State state)
        {
            state.EnemyHealth -= 10;
            if (state.EnemyHealth <= 0)
            {
                state.EnemyHealth = 0;
            }

            return state;
        }

        private State PerformEnemyTurn(State state)
        {
            int enemyAttackValue = 5;
            state.PlayerHealth -= enemyAttackValue;
            if (state.PlayerHealth < 0)
            {
                state.PlayerHealth = 0;
            }
            return state;
        }
    }
}
