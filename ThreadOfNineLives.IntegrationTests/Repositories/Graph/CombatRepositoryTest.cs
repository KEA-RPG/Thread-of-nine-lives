using Backend.Repositories.Graph;
using Backend.Repositories.Interfaces;
using Backend.Repositories.Relational;
using Domain.DTOs;
using Infrastructure.Persistance;
using Infrastructure.Persistance.Document;

namespace ThreadOfNineLives.IntegrationTests.Repositories.Graph
{
    public class DocumentCombatRepositoryTest
    {
        private readonly ICombatRepository _combatRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEnemyRepository _enemyRepository;
        public DocumentCombatRepositoryTest()
        {
            var _context = PersistanceConfiguration.GetGraphContext(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
            _combatRepository = new GraphCombatRepository(_context);
            _userRepository = new GraphUserRepository(_context);
            _enemyRepository = new GraphEnemyRepository(_context);
        }

        [Fact]
        public void AddFight_Assigns_Id_And_Retains_Values()
        {
            //Arrange
            var testEnemy = _enemyRepository.GetAllEnemies().FirstOrDefault();
            if (testEnemy == null)
            {
                var user = new EnemyDTO()
                {
                    Health = 100,
                    ImagePath = "test",
                    Name = "testEnemy"
                };
                _enemyRepository.AddEnemy(user);
                testEnemy = _enemyRepository.GetAllEnemies().FirstOrDefault();
            }

            var testUser = _userRepository.GetUserByUsername("testuser");
            if (testUser == null)
            {
                var user = new UserDTO()
                {
                    Password = "test",
                    Username = "test",
                    Role = "test"
                };
                _userRepository.CreateUser(user);
                testUser = _userRepository.GetUserByUsername(user.Username);
            }
            var testFight = new FightDTO()
            {
                EnemyId = testEnemy.Id,
                UserId = testUser.Id,
                GameActions = new List<GameActionDTO>()
            };
            // act
            var data = _combatRepository.AddFight(testFight);


            //Assert
            Assert.NotNull(data);
            Assert.True(data.Id > 0);
            Assert.Equal(data.EnemyId, testFight.EnemyId);
            Assert.Equal(data.UserId, testFight.UserId);
            Assert.Equal(data.GameActions, testFight.GameActions);

        }

        [Fact]
        public void GetFightById_Returns_Correct_Fight()
        {
            // Arrange
            var createdFight = CreateTemplateFightAndActions();

            // Act
            var data = _combatRepository.GetFightById(createdFight.Id);

            // Assert
            Assert.NotNull(data);
            Assert.Equal(createdFight.Id, data.Id);
            Assert.Equal(createdFight.EnemyId, data.EnemyId);
            Assert.Equal(createdFight.UserId, data.UserId);
            Assert.Equal(createdFight.GameActions.Count, data.GameActions.Count);
        }

        [Fact]
        public void InsertAction_Updates_Fight_With_GameAction()
        {
            //Arrange
            var createdFight = CreateTemplateFightAndActions();
            var gameAction = new GameActionDTO()
            {
                Type = "ATTACK",
                Value = 4,
                FightId = createdFight.Id,
            };

            //Act
            _combatRepository.InsertAction(gameAction);
            var dbFight = _combatRepository.GetFightById(createdFight.Id);

            //Assert
            Assert.NotEmpty(dbFight.GameActions);
            Assert.True(dbFight.GameActions.First().Type == gameAction.Type);
            Assert.True(dbFight.GameActions.First().Value == gameAction.Value);
            Assert.True(dbFight.GameActions.First().FightId == gameAction.FightId);


        }

        [Fact]
        public void InsertAction_Updates_Fight_With_GameAction_With_Multiple_GameActions()
        {
            //Arrange
            var createdFight = CreateTemplateFightAndActions();
            var gameAction = new GameActionDTO()
            {
                Type = "ATTACK",
                Value = 4,
                FightId = createdFight.Id,
            };
            var gameAction2 = new GameActionDTO()
            {
                Type = "ATTACK",
                Value = 45,
                FightId = createdFight.Id,
            };

            //Act
            _combatRepository.InsertAction(gameAction);
            _combatRepository.InsertAction(gameAction2);
            var dbFight = _combatRepository.GetFightById(createdFight.Id);

            //Assert
            Assert.NotEmpty(dbFight.GameActions);
            Assert.True(dbFight.GameActions.Count == 2);
        }

        [Fact]
        public void GetFightById_Returns_Null_When_Id_Does_Not_Exist()
        {
            // Arrange & Act
            var retrievedFight = _combatRepository.GetFightById(-1);

            // Assert
            Assert.Null(retrievedFight);
        }
        private FightDTO CreateTemplateFightAndActions()
        {
            var testEnemy = _enemyRepository.GetAllEnemies().FirstOrDefault();
            if (testEnemy == null)
            {
                var user = new EnemyDTO()
                {
                    Health = 100,
                    ImagePath = "test",
                    Name = "testEnemy"
                };
                _enemyRepository.AddEnemy(user);
                testEnemy = _enemyRepository.GetAllEnemies().FirstOrDefault();
            }

            var testUser = _userRepository.GetUserByUsername("testuser");
            if (testUser == null)
            {
                var user = new UserDTO()
                {
                    Password = "test",
                    Username = "test",
                    Role = "test"
                };
                _userRepository.CreateUser(user);
                testUser = _userRepository.GetUserByUsername(user.Username);
            }
            var testFight = new FightDTO()
            {
                EnemyId = testEnemy.Id,
                UserId = testUser.Id,
                GameActions = new List<GameActionDTO>()
            };
            return _combatRepository.AddFight(testFight);


        }
    }
}
