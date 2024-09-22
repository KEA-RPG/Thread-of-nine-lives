using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Repositories;
using Backend.Services;
using Domain.Entities;
using Moq;
using Xunit;

namespace UnitTests
{
    public class EnemyUnit
    {
        //Outer Region for positive tests
        #region Positive Tests 

        //CreateEnemies for testing
        private IEnumerable<Enemy> GetTestEnemies(int count)
        {
            var enemies = new List<Enemy>();
            for (int i = 0; i < count; i++)
            {
                enemies.Add(new Enemy
                {
                    Id = i,
                    Name = $"Enemy {i}",
                    Health = $"{i * 100} HP", // Health as a string (per the Enemy class)
                    ImagePath = $"ImagePath_{i}.jpg"
                });
            }
            return enemies;
        }

        #region Entity
        // You can add any specific tests for the entity if required
        #endregion

        #region Repository
        // Tests specific to the repository can go here if needed
        #endregion

        #region Service

        [Fact]
        public void GetEnemyById_Should_Return_A_Single_Enemy()
        {
            //Arrange
            var mockEnemyRepository = new Mock<IEnemyRepository>();
            var enemy = new Enemy
            {
                Id = 1,
                Name = "Enemy 1",
                Health = "100 HP",
                ImagePath = "ImagePath_1.jpg"
            };

            //Mocking the GetEnemyById method
            mockEnemyRepository.Setup(repo => repo.GetEnemyById(1)).Returns(enemy);

            var enemyService = new EnemyService(mockEnemyRepository.Object);

            //Act
            var result = enemyService.GetEnemyById(1);

            //Assert
            Assert.Equal(enemy, result);
            mockEnemyRepository.Verify(repo => repo.GetEnemyById(1), Times.Once);
        }

        [Theory]
        [InlineData(1)] //If there should only be 1 enemy
        [InlineData(3)] //If there should be more than 1 enemies
        public void GetAllEnemies_Should_Return_All_Enemies(int count)
        {
            //Arrange
            var mockEnemyRepository = new Mock<IEnemyRepository>();
            var enemies = GetTestEnemies(count);

            mockEnemyRepository.Setup(repo => repo.GetAllEnemies()).Returns(enemies.ToList());

            var enemyService = new EnemyService(mockEnemyRepository.Object);

            //Act
            var result = enemyService.GetAllEnemies();

            //Assert
            Assert.Equal(enemies, result);
        }

        [Fact]
        public void CreateEnemy_Should_Create_A_New_Enemy()
        {
            //Arrange
            var mockEnemyRepository = new Mock<IEnemyRepository>();
            var enemy = new Enemy
            {
                Id = 1,
                Name = "Enemy 1",
                Health = "100 HP",
                ImagePath = "ImagePath_1.jpg"
            };

            // Since AddEnemy is a void method, we use Callback instead of Returns
            mockEnemyRepository.Setup(repo => repo.AddEnemy(enemy)).Callback<Enemy>(e =>
            {
                // We can optionally assert that the AddEnemy method is called with the correct enemy
                Assert.Equal(enemy, e);
            });

            var enemyService = new EnemyService(mockEnemyRepository.Object);

            //Act
            enemyService.CreateEnemy(enemy);

            //Assert
            mockEnemyRepository.Verify(repo => repo.AddEnemy(enemy), Times.Once);
        }

        #endregion

        #region Controller
        // Controller-related tests can go here if needed
        #endregion

        #endregion
    }
}
