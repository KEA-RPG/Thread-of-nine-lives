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

            _enemyRepositoryMock
                .Setup(repo => repo.GetEnemyById(stateGameInit.EnemyId))
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

            _userRepositoryMock
                .Setup(repo => repo.GetUserByUsername(stateGameInit.UserName))
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

            var userDto = new UserDTO
            {
                Id = 123,
                Username = stateGameInit.UserName,
                Password = "someHash",
                Role = "Player"
            };
            _userRepositoryMock
                .Setup(repo => repo.GetUserByUsername(stateGameInit.UserName))
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

            _userRepositoryMock
                .Setup(repo => repo.GetUserByUsername(stateGameInit.UserName))
                .Returns(new UserDTO
                {
                    Id = 7,
                    Username = stateGameInit.UserName,
                    Password = "someHash",
                    Role = "Player"
                });

            FightDTO fightDto = null; 
            _combatRepositoryMock
                .Setup(repo => repo.AddFight(It.IsAny<FightDTO>()))
                .Returns((FightDTO fight) =>
                {
                    fight.Id = 777;
                    fightDto = fight; 
                    return fight;
                });

            // Act
            var result = _combatService.GetInitialState(stateGameInit);

            // Assert
            Assert.Equal(777, result.FightId);
            Assert.NotNull(fightDto); 
        }

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

            var fightBefore = new FightDTO { Id = fightID };

            var fightAfter = new FightDTO
            {
                Id = fightID,
                GameActions = new List<GameActionDTO> { action }
            };

            _combatRepositoryMock
                .SetupSequence(repo => repo.GetFightById(fightID))
                .Returns(fightBefore)
                .Returns(fightAfter);

            // Act
            var state = _combatService.GetProcessedState(fightID, action);

            // Assert 
            Assert.Equal(fightID, state.FightId);
        }


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

            // Assert 
            Assert.Equal(fightId, state.FightId);
        }
    }
}
