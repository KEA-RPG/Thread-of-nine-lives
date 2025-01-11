using System;
using System.Collections.Generic;
using Backend.Helpers;
using Backend.Models;
using Backend.Repositories.Interfaces;
using Backend.Services;
using Domain.DTOs;
using Domain.Entities;
using Moq;
using Xunit;

namespace Backend.Tests
{
    public class CombatServiceTests
    {
        private readonly Mock<ICombatRepository> _combatRepositoryMock;
        private readonly Mock<IEnemyRepository> _enemyRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly CombatService _combatService;

        public CombatServiceTests()
        {
            _combatRepositoryMock = new Mock<ICombatRepository>();
            _enemyRepositoryMock = new Mock<IEnemyRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();

            _combatService = new CombatService(
                _combatRepositoryMock.Object,
                _enemyRepositoryMock.Object,
                _userRepositoryMock.Object
            );
        }

        // ---------------------------------------------------------
        // GetInitialState(...)
        // ---------------------------------------------------------

        [Fact]
        public void GetInitialState_NullStateGameInit_ThrowsArgumentNullException()
        {
            // Arrange
            StateGameInit init = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _combatService.GetInitialState(init));
        }

        [Fact]
        public void GetInitialState_EnemyNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var stateGameInit = new StateGameInit
            {
                EnemyId = 123,
                UserName = "TestUser"
            };

            // Enemy doesn't exist
            _enemyRepositoryMock
                .Setup(repo => repo.GetEnemyById(123))
                .Returns((EnemyDTO)null);

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _combatService.GetInitialState(stateGameInit));
        }

        [Fact]
        public void GetInitialState_UserNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var stateGameInit = new StateGameInit
            {
                EnemyId = 999,
                UserName = "MissingUser"
            };

            // Enemy *does* exist
            _enemyRepositoryMock
                .Setup(repo => repo.GetEnemyById(999))
                .Returns(new EnemyDTO
                {
                    Id = 999,
                    Name = "SomeEnemy",
                    Health = 100,
                    ImagePath = "test.png"
                });

            // UserDTO is null => not found
            _userRepositoryMock
                .Setup(repo => repo.GetUserByUsername("MissingUser"))
                .Returns((UserDTO)null);

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _combatService.GetInitialState(stateGameInit));
        }

        [Fact]
        public void GetInitialState_CallsAddFightOnce()
        {
            // Arrange
            var stateGameInit = new StateGameInit
            {
                EnemyId = 100,
                UserName = "TestUser"
            };

            _enemyRepositoryMock
                .Setup(repo => repo.GetEnemyById(100))
                .Returns(new EnemyDTO
                {
                    Id = 100,
                    Name = "SomeEnemy",
                    Health = 100,
                    ImagePath = "test.png"
                });

            // Return a valid UserDTO
            var userDto = new UserDTO
            {
                Id = 123,
                Username = "TestUser",
                Password = "someHash",
                Role = "Player"
            };
            _userRepositoryMock
                .Setup(repo => repo.GetUserByUsername("TestUser"))
                .Returns(userDto);

            // AddFight mock
            _combatRepositoryMock
                .Setup(repo => repo.AddFight(It.IsAny<FightDTO>()))
                .Returns((FightDTO fight) =>
                {
                    fight.Id = 999;
                    return fight;
                });

            // Act
            _combatService.GetInitialState(stateGameInit);

            // Assert
            _combatRepositoryMock.Verify(
                repo => repo.AddFight(It.IsAny<FightDTO>()),
                Times.Once
            );
        }

        [Fact]
        public void GetInitialState_ReturnsStateFromStateFactory()
        {
            // Arrange
            var stateGameInit = new StateGameInit
            {
                EnemyId = 100,
                UserName = "TestUser"
            };

            _enemyRepositoryMock
                .Setup(repo => repo.GetEnemyById(100))
                .Returns(new EnemyDTO
                {
                    Id = 100,
                    Name = "SomeEnemy",
                    Health = 100,
                    ImagePath = "test.png"
                });

            // Return a valid UserDTO
            _userRepositoryMock
                .Setup(repo => repo.GetUserByUsername("TestUser"))
                .Returns(new UserDTO
                {
                    Id = 7,
                    Username = "TestUser",
                    Password = "someHash",
                    Role = "Player"
                });

            // AddFight mock
            _combatRepositoryMock
                .Setup(repo => repo.AddFight(It.IsAny<FightDTO>()))
                .Returns((FightDTO fight) =>
                {
                    fight.Id = 777;
                    return fight;
                });

            // Act
            var result = _combatService.GetInitialState(stateGameInit);

            // Assert (single check)
            Assert.Equal(777, result.FightId);
        }

        // ---------------------------------------------------------
        // GetProcessedState(...)
        // ---------------------------------------------------------

        [Fact]
        public void GetProcessedState_ActionIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            int fightID = 99;
            GameActionDTO nullAction = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => _combatService.GetProcessedState(fightID, nullAction)
            );
        }

        [Fact]
        public void GetProcessedState_FightNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            int fightID = 123;
            var action = new GameActionDTO { Type = "ATTACK", Value = 5 };

            _combatRepositoryMock
                .Setup(repo => repo.GetFightById(fightID))
                .Returns((FightDTO)null);

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(
                () => _combatService.GetProcessedState(fightID, action)
            );
        }

        [Fact]
        public void GetProcessedState_CallsInsertActionOnce()
        {
            // Arrange
            int fightID = 10;
            var action = new GameActionDTO { Type = "ATTACK", Value = 3 };

            var existingFight = new FightDTO { Id = fightID };
            _combatRepositoryMock
                .Setup(repo => repo.GetFightById(fightID))
                .Returns(existingFight);

            // Act
            _combatService.GetProcessedState(fightID, action);

            // Assert
            _combatRepositoryMock.Verify(
                repo => repo.InsertAction(action),
                Times.Once
            );
        }

        [Fact]
        public void GetProcessedState_ReturnsUpdatedStateFromStateFactory()
        {
            // Arrange
            int fightID = 45;
            var action = new GameActionDTO { Type = "END_TURN", Value = 0 };

            // The fight before adding the action:
            var fightBefore = new FightDTO { Id = fightID };

            // The fight after adding the action (if InsertAction modifies it or if we fetch it again):
            var fightAfter = new FightDTO
            {
                Id = fightID,
                GameActions = new List<GameActionDTO> { action }
            };

            // Set up the sequence: first call returns fightBefore, second call returns fightAfter
            _combatRepositoryMock
                .SetupSequence(repo => repo.GetFightById(fightID))
                .Returns(fightBefore)
                .Returns(fightAfter);

            // Act
            var state = _combatService.GetProcessedState(fightID, action);

            // Assert (single check)
            Assert.Equal(fightID, state.FightId);
        }

        // ---------------------------------------------------------
        // GetStateByFightId(...)
        // ---------------------------------------------------------

        [Fact]
        public void GetStateByFightId_FightNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            int missingFightId = 999;

            _combatRepositoryMock
                .Setup(repo => repo.GetFightById(missingFightId))
                .Returns((FightDTO)null);

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(
                () => _combatService.GetStateByFightId(missingFightId)
            );
        }

        [Fact]
        public void GetStateByFightId_ReturnsState()
        {
            // Arrange
            int fightId = 77;
            var fightDTO = new FightDTO { Id = fightId };

            _combatRepositoryMock
                .Setup(repo => repo.GetFightById(fightId))
                .Returns(fightDTO);

            // Act
            var state = _combatService.GetStateByFightId(fightId);

            // Assert (single check)
            Assert.Equal(fightId, state.FightId);
        }
    }
}
