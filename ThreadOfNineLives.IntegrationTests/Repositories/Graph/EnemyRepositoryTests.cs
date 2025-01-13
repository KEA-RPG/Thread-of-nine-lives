using Backend.Repositories.Document;
using Backend.Repositories.Graph;
using Backend.Repositories.Interfaces;
using Domain.DTOs;
using Infrastructure.Persistance;
using Infrastructure.Persistance.Document;

namespace ThreadOfNineLives.IntegrationTests.Repositories.Graph
{
    public class EnemyRepositoryTests
    {
        private readonly IEnemyRepository _enemyRepository;
        public EnemyRepositoryTests()
        {
            var _context = PersistanceConfiguration.GetGraphContext(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
            _enemyRepository = new GraphEnemyRepository(_context);
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
            var data = _enemyRepository.AddEnemy(testEnemy);

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
            var dbEnemy = _enemyRepository.AddEnemy(testEnemy);

            //Act
            _enemyRepository.DeleteEnemy(dbEnemy);
            var data = _enemyRepository.GetEnemyById(dbEnemy.Id);

            //Assert
            Assert.Null(data);
        }

        [Fact]
        public void GetAllEnemies_Returns_List_Of_Enemies()
        {
            //Arrange

            var testEnemy = new EnemyDTO()
            {
                Health = 1,
                ImagePath = 1.ToString(),
                Name = 1.ToString()
            };
            _enemyRepository.AddEnemy(testEnemy);

            //Act
            var data = _enemyRepository.GetAllEnemies();

            //Assert
            Assert.NotNull(data);
            Assert.NotEmpty(data);
        }

        [Fact]
        public void GetEnemyById_Returns_Null_Given_Invalid_Id()
        {
            // Arrange & Act
            var data = _enemyRepository.GetEnemyById(-1);

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
            var dbEnemy = _enemyRepository.AddEnemy(testEnemy);


            //Act
            var data = _enemyRepository.GetEnemyById(dbEnemy.Id);

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
            var dbEnemy = _enemyRepository.AddEnemy(testEnemy);
            dbEnemy.Health = 11;

            //Act
            _enemyRepository.UpdateEnemy(dbEnemy);
            var data = _enemyRepository.GetEnemyById(dbEnemy.Id);

            //Assert
            Assert.NotNull(data);
            Assert.Equal(data.Health, dbEnemy.Health);
        }
    }
}
