using System;
using System.Collections.Generic;
using Xunit;
using Backend.Helpers;
using Backend.Models;
using Domain.DTOs;
using Domain.Entities;

namespace Backend.Tests
{
    public class StateFactoryTests
    {
        private readonly StateFactory _factory;

        public StateFactoryTests()
        {
            _factory = new StateFactory();
        }

        [Fact]
        public void CreateState_EnemyIsNull_SetsDefaultEnemyHealthTo50()
        {
            // Arrange
            var fight = new FightDTO
            {
                Id = 1,
                Enemy = null,
                GameActions = new List<GameActionDTO>()
            };

            // Act
            State result = _factory.CreateState(fight);

            // Assert
            Assert.Equal(50, result.EnemyHealth);
        }

        [Fact]
        public void CreateState_EnemyIsNotNull_UsesEnemyHealthFromEnemyDTO()
        {
            // Arrange
            var fight = new FightDTO
            {
                Id = 2,
                Enemy = new EnemyDTO
                {
                    Id = 5,
                    Name = "Test Enemy",
                    Health = 60,
                    ImagePath = "test.png"
                },
                GameActions = new List<GameActionDTO>()
            };

            // Act
            State result = _factory.CreateState(fight);

            // Assert
            Assert.Equal(60, result.EnemyHealth);
        }

        [Fact]
        public void CreateState_DefaultPlayerHealthIs25()
        {
            // Arrange
            var fight = new FightDTO
            {
                Id = 10,
                Enemy = new EnemyDTO
                {
                    Id = 999,
                    Name = "Any Enemy",
                    Health = 77,
                    ImagePath = "enemy.png"
                }
            };

            // Act
            State result = _factory.CreateState(fight);

            // Assert
            Assert.Equal(25, result.PlayerHealth);
        }

        [Fact]
        public void CreateState_GameActionsIsNull_DoesNotThrowException()
        {
            // Arrange
            var fight = new FightDTO
            {
                Id = 3,
                Enemy = new EnemyDTO
                {
                    Id = 6,
                    Name = "Null Actions Enemy",
                    Health = 40,
                    ImagePath = "none.png"
                },
                GameActions = null // Null
            };

            // Act
            var exception = Record.Exception(() => _factory.CreateState(fight));

            // Assert
            Assert.Null(exception);
        }


        [Fact]
        public void CreateState_AttackAction_ReducesEnemyHealthByValue()
        {
            // Arrange
            var fight = new FightDTO
            {
                Id = 4,
                Enemy = new EnemyDTO
                {
                    Id = 7,
                    Name = "Goblin",
                    Health = 20,
                    ImagePath = "goblin.png"
                },
                GameActions = new List<GameActionDTO>
                {
                    new GameActionDTO { Type = "ATTACK", Value = 5 }
                }
            };

            // Act
            State result = _factory.CreateState(fight);

            // Assert
            Assert.Equal(15, result.EnemyHealth); // 20 -> 15
        }

        [Fact]
        public void CreateState_AttackAction_EnemyHealthCannotGoBelowZero()
        {
            // Arrange
            var fight = new FightDTO
            {
                Id = 5,
                Enemy = new EnemyDTO
                {
                    Id = 8,
                    Name = "Skeleton",
                    Health = 10,
                    ImagePath = "skeleton.png"
                },
                GameActions = new List<GameActionDTO>
                {
                    new GameActionDTO { Type = "ATTACK", Value = 15 } // Overkill
                }
            };

            // Act
            State result = _factory.CreateState(fight);

            // Assert
            Assert.Equal(0, result.EnemyHealth);
        }

        [Fact]
        public void CreateState_EndTurnAction_ReducesPlayerHealthByFive()
        {
            // Arrange
            var fight = new FightDTO
            {
                Id = 6,
                Enemy = new EnemyDTO
                {
                    Id = 9,
                    Name = "Wolf",
                    Health = 30,
                    ImagePath = "wolf.png"
                },
                GameActions = new List<GameActionDTO>
                {
                    new GameActionDTO { Type = "END_TURN", Value = 0 }
                }
            };

            // Act
            State result = _factory.CreateState(fight);

            // Assert
            Assert.Equal(20, result.PlayerHealth); // 25 -> 20
        }

        [Fact]
        public void CreateState_EndTurnAction_PlayerHealthCannotGoBelowZero()
        {
            // Arrange
            var fight = new FightDTO
            {
                Id = 7,
                Enemy = new EnemyDTO
                {
                    Id = 10,
                    Name = "Orc",
                    Health = 40,
                    ImagePath = "orc.png"
                },
                GameActions = new List<GameActionDTO>
                {
                    // Each END_TURN does 5 damage
                    new GameActionDTO { Type = "END_TURN" },
                    new GameActionDTO { Type = "END_TURN" },
                    new GameActionDTO { Type = "END_TURN" },
                    new GameActionDTO { Type = "END_TURN" },
                    new GameActionDTO { Type = "END_TURN" },
                    new GameActionDTO { Type = "END_TURN" }
                }
            };

            // Act
            State result = _factory.CreateState(fight);

            // Assert
            // Player starts at 25, total 6 x 5 = 30 damage => below zero => clamp to 0
            Assert.Equal(0, result.PlayerHealth);
        }

        [Fact]
        public void CreateState_ProcessMultipleActionsInOrder()
        {
            // Arrange
            var fight = new FightDTO
            {
                Id = 8,
                Enemy = new EnemyDTO
                {
                    Id = 11,
                    Name = "Dragon Whelp",
                    Health = 50,
                    ImagePath = "dragon.png"
                },
                GameActions = new List<GameActionDTO>
                {
                    new GameActionDTO { Type = "ATTACK", Value = 10 }, // Enemy: 50 -> 40
                    new GameActionDTO { Type = "END_TURN" },           // Player: 25 -> 20
                    new GameActionDTO { Type = "ATTACK", Value = 5  }, // Enemy: 40 -> 35
                    new GameActionDTO { Type = "END_TURN" }            // Player: 20 -> 15
                }
            };

            // Act
            State result = _factory.CreateState(fight);

            // Assert
            Assert.Equal(35, result.EnemyHealth);
            Assert.Equal(15, result.PlayerHealth);
        }
    }
}
