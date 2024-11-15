using Backend.Models;
using Domain.DTOs;
using Domain.Entities;

namespace Backend.Helpers
{
    public class StateFactory
    {
        public State CreateInitState()
        {
            int playerHealth = 25;
            int enemyHealth = 50;
            State state = new State
            {
                GameActions = new List<GameAction>(),
                PlayerHealth = playerHealth,
                EnemyHealth = enemyHealth
            };

            return state;
        }

        public State ProcessAction (GameActionDTO gameAction, State state)
        {
            if (gameAction.Type == "ATTACK")
            {
                PerformPlayerAttack(state);
            }
            else if (gameAction.Type == "END_TURN")
            {
                PerformEnemyTurn(state);
            }
            return state;
        }

        private static void PerformPlayerAttack(State state)
        {
            state.EnemyHealth -= 10;
            if (state.EnemyHealth <= 0)
            {
                state.EnemyHealth = 0;
            }
        }

        private static void PerformEnemyTurn(State state)
        {
            int enemyAttackValue = 5;
            state.PlayerHealth -= enemyAttackValue;
            if (state.PlayerHealth < 0)
            {
                state.PlayerHealth = 0;
            }

        }
    }
}
