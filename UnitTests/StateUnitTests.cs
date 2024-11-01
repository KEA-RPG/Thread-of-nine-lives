using Backend.Services;
using Domain.DTOs;
using Domain.Entities;

namespace UnitTests
{
    public class StateUnitTests
    {

        private PlayerDTO CreateTestPlayer(int health = 100)
        {
            return new PlayerDTO { Health = health };
        }

        private EnemyDTO CreateTestEnemy(int health = 50)
        {
            return new EnemyDTO { Health = health, Name = "Test Enemy", ImagePath = "test_image_path.png" };
        }

        [Fact]
        public void ProcessAction_AttackAction_ReducesEnemyHealth()
        {
            // Arrange
            var playerDTO = CreateTestPlayer();
            var enemyDTO = CreateTestEnemy();
            var state = new State(playerDTO, enemyDTO);
            var gameAction = new GameAction { Type = "ATTACK", Value = 10 };

            // Act
            state.ProcessAction(gameAction);

            // Assert
            Assert.Equal(40, state.EnemyDTO.Health);
        }

        [Fact]
        public void ProcessAction_AttackAction_EnemyHealthNotBelowZero()
        {
            // Arrange
            var playerDTO = CreateTestPlayer();
            var enemyDTO = CreateTestEnemy(5);
            var state = new State(playerDTO, enemyDTO);
            var gameAction = new GameAction { Type = "ATTACK", Value = 10 };

            // Act
            state.ProcessAction(gameAction);

            // Assert
            Assert.Equal(0, state.EnemyDTO.Health);
        }

        [Fact]
        public void ProcessAction_EndTurn_EnemyAttacksAndPlayerMinus5Health()
        {
            // Arrange
            var playerDTO = CreateTestPlayer();
            var enemyDTO = CreateTestEnemy();
            var state = new State(playerDTO, enemyDTO);
            var gameAction = new GameAction { Type = "END_TURN", Value = 0 };

            // Act
            state.ProcessAction(gameAction);

            // Assert
            Assert.Equal(95, state.PlayerDTO.Health);
        }

        [Fact]
        public void ProcessAction_PlayerHealthNotBelowZero()
        {
            // Arrange
            var playerDTO = CreateTestPlayer(3);
            var enemyDTO = CreateTestEnemy();
            var state = new State(playerDTO, enemyDTO);

            // Act
            state.ProcessAction(new GameAction { Type = "END_TURN", Value = 0 });

            // Assert
            Assert.Equal(0, state.PlayerDTO.Health);
        }
    }
}
