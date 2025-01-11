using Backend.Repositories.Document;
using Domain.DTOs;
using Infrastructure.Persistance.Document;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadOfNineLives.IntegrationTests.DocumentDB
{
    public class DocumentCombatRepositoryTest : IDisposable
    {
        private readonly DocumentContext _context;
        private readonly MongoCombatRepository _mongoCombatRepository;
        private readonly DatabaseSnapshotHelper _snapshotHelper;
        public DocumentCombatRepositoryTest()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("dbsettings.json")
                .Build();

            var settings = configuration.GetSection("ConnectionStrings:MongoDB");
            var connectionString = settings.GetSection("Connectionstring").Value;
            var databaseName = settings.GetSection("DatabaseName").Value;

            _context = new DocumentContext(connectionString, databaseName);
            _mongoCombatRepository = new MongoCombatRepository(_context);
            _snapshotHelper = new DatabaseSnapshotHelper(_context);

            _snapshotHelper.TakeSnapshot();
        }

        [Fact]
        public void AddFight_Assigns_Id_And_Retains_Values()
        {
            //Arrange
            var testFight = new FightDTO()
            {
                EnemyId = 123,
                UserId = 123,
                GameActions = new List<GameActionDTO>()
            };

            //Act
            var data = _mongoCombatRepository.AddFight(testFight);

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
            var data = _mongoCombatRepository.GetFightById(createdFight.Id);

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
            _mongoCombatRepository.InsertAction(gameAction);
            var dbFight = _mongoCombatRepository.GetFightById(createdFight.Id);

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
            _mongoCombatRepository.InsertAction(gameAction);
            _mongoCombatRepository.InsertAction(gameAction2);
            var dbFight = _mongoCombatRepository.GetFightById(createdFight.Id);

            //Assert
            Assert.NotEmpty(dbFight.GameActions);
            Assert.True(dbFight.GameActions.Count == 2);
        }

        [Fact]
        public void GetFightById_Returns_Null_When_Id_Does_Not_Exist()
        {
            // Arrange
            _context.Fights().DeleteMany(FilterDefinition<FightDTO>.Empty);

            // Act
            var retrievedFight = _mongoCombatRepository.GetFightById(9999);

            // Assert
            Assert.Null(retrievedFight);
        }
        private FightDTO CreateTemplateFightAndActions()
        {
            var testFight = new FightDTO()
            {
                EnemyId = 123,
                UserId = 123,
                GameActions = new List<GameActionDTO>()
            };
            return _mongoCombatRepository.AddFight(testFight);


        }
        public void Dispose()
        {
            _snapshotHelper.RestoreSnapshot();
        }
    }
}
