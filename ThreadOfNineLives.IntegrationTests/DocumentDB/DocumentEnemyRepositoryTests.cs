using Backend.Repositories.Document;
using Domain.DTOs;
using Infrastructure.Persistance;
using Infrastructure.Persistance.Document;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadOfNineLives.IntegrationTests.DocumentDB
{
    public class DocumentEnemyRepositoryTests : IDisposable
    {
        private readonly DocumentContext _context;
        private readonly MongoEnemyRepository _mongoEnemyRepository;
        private readonly DatabaseSnapshotHelper _snapshotHelper;
        public DocumentEnemyRepositoryTests()
        {
            _context = PersistanceConfiguration.GetDocumentContext(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
            _mongoEnemyRepository = new MongoEnemyRepository(_context);
            _snapshotHelper = new DatabaseSnapshotHelper(_context);

            _snapshotHelper.TakeSnapshot();
        }

        [Fact]
        public void AddEnemy_Assigns_Id_And_Retains_Values()
        {
            //Arrange
            var testEnemy = new EnemyDTO()
            {
                Health = 10,
                ImagePath = "test",
                Name = "test"
            };

            //Act
            var data = _mongoEnemyRepository.AddEnemy(testEnemy);

            //Assert
            Assert.NotNull(data);
            Assert.True(data.Id > 0);
            Assert.Equal(data.Health, testEnemy.Health);
            Assert.Equal(data.ImagePath, testEnemy.ImagePath);
            Assert.Equal(data.Name, testEnemy.Name);

        }

        [Fact]
        public void DeleteEnemy_Deletes_Entity()
        {
            //Arrange
            var testEnemy = new EnemyDTO()
            {
                Health = 10,
                ImagePath = "test",
                Name = "test"
            };
            var dbEnemy = _mongoEnemyRepository.AddEnemy(testEnemy);

            //Act
            _mongoEnemyRepository.DeleteEnemy(dbEnemy);
            var data = _mongoEnemyRepository.GetEnemyById(dbEnemy.Id);

            //Assert
            Assert.Null(data);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(99)]

        public void GetAllEnemies_Returns_List_Of_Enemies(int insertedEnemiesAmount)
        {
            //Arrange
            _context.Enemies().DeleteMany(FilterDefinition<EnemyDTO>.Empty);

            for (int i = 0; i < insertedEnemiesAmount; i++)
            {
                var testEnemy = new EnemyDTO()
                {
                    Health = i,
                    ImagePath = i.ToString(),
                    Name = i.ToString()
                };
                _mongoEnemyRepository.AddEnemy(testEnemy);

            }

            //Act
            var data = _mongoEnemyRepository.GetAllEnemies();

            //Assert
            Assert.NotNull(data);
            Assert.Equal(data.Count, insertedEnemiesAmount);
        }

        [Fact]
        public void GetEnemyById_Returns_Null_Given_Invalid_Id()
        {
            //Arrange
            _context.Enemies().DeleteMany(FilterDefinition<EnemyDTO>.Empty);


            //Act
            var data = _mongoEnemyRepository.GetEnemyById(123);

            //Assert
            Assert.Null(data);
        }
        [Fact]
        public void GetEnemyById_Returns_Correct_Data_Given_Id()
        {
            //Arrange
            var testEnemy = new EnemyDTO()
            {
                Health = 10,
                ImagePath = "test",
                Name = "test"
            };
            var dbEnemy = _mongoEnemyRepository.AddEnemy(testEnemy);


            //Act
            var data = _mongoEnemyRepository.GetEnemyById(dbEnemy.Id);

            //Assert
            Assert.NotNull(data);
            Assert.True(data.Id > 0);
            Assert.Equal(data.Health, testEnemy.Health);
            Assert.Equal(data.ImagePath, testEnemy.ImagePath);
            Assert.Equal(data.Name, testEnemy.Name);
        }
        [Fact]
        public void UpdateEnemy_Sets_Property()
        {
            //Arrange
            var testEnemy = new EnemyDTO()
            {
                Health = 10,
                ImagePath = "test",
                Name = "test"
            };
            var dbEnemy = _mongoEnemyRepository.AddEnemy(testEnemy);
            dbEnemy.Health = 11;

            //Act
            _mongoEnemyRepository.UpdateEnemy(dbEnemy);
            var data = _mongoEnemyRepository.GetEnemyById(dbEnemy.Id);

            //Assert
            Assert.NotNull(data);
            Assert.Equal(data.Health, dbEnemy.Health);
        }

        public void Dispose()
        {
            _snapshotHelper.RestoreSnapshot();
        }
    }
}
