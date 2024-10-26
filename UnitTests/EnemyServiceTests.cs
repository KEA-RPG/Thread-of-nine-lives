using Xunit;
using Moq;
using Backend.Services;
using Backend.Repositories;
using Domain.Entities;
using Domain.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Tests.Services
{
    public class EnemyServiceTests
    {
        private readonly EnemyService _enemyService;
        private readonly Mock<IEnemyRepository> _mockRepository;

        public EnemyServiceTests()
        {
            _mockRepository = new Mock<IEnemyRepository>();
            _enemyService = new EnemyService(_mockRepository.Object);
        }

        // GetEnemyById Tests

        [Fact]
        public void GetEnemyById_ExistingId_ReturnsNonNullResult()
        {
            // Arrange
            int enemyId = 1;
            var enemy = new Enemy { Id = enemyId };
            _mockRepository.Setup(repo => repo.GetEnemyById(enemyId)).Returns(enemy);

            // Act
            var result = _enemyService.GetEnemyById(enemyId);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetEnemyById_ExistingId_ReturnsCorrectEnemyDTO()
        {
            // Arrange
            int enemyId = 1;
            var enemy = new Enemy
            {
                Id = enemyId,
                Name = "Goblin",
                Health = 100,
                ImagePath = "/images/goblin.png"
            };
            _mockRepository.Setup(repo => repo.GetEnemyById(enemyId)).Returns(enemy);

            // Act
            var result = _enemyService.GetEnemyById(enemyId);

            // Assert
            Assert.Equal(enemyId, result.Id);
            Assert.Equal("Goblin", result.Name);
            Assert.Equal(100, result.Health);
            Assert.Equal("/images/goblin.png", result.ImagePath);
        }

        [Fact]
        public void GetEnemyById_NonExistingId_ReturnsNull()
        {
            // Arrange
            int enemyId = 1;
            _mockRepository.Setup(repo => repo.GetEnemyById(enemyId)).Returns((Enemy)null);

            // Act
            var result = _enemyService.GetEnemyById(enemyId);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(999)]
        public void GetEnemyById_NonExistingIds_ReturnsNull(int enemyId)
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetEnemyById(enemyId)).Returns((Enemy)null);

            // Act
            var result = _enemyService.GetEnemyById(enemyId);

            // Assert
            Assert.Null(result);
        }

        // GetAllEnemies Tests

        [Fact]
        public void GetAllEnemies_WhenEnemiesExist_ReturnsNonEmptyList()
        {
            // Arrange
            var enemies = new List<Enemy>
            {
                new Enemy { Id = 1, Name = "Goblin", Health = 100, ImagePath = "/images/goblin.png" },
                new Enemy { Id = 2, Name = "Orc", Health = 200, ImagePath = "/images/orc.png" }
            };
            _mockRepository.Setup(repo => repo.GetAllEnemies()).Returns(enemies);

            // Act
            var result = _enemyService.GetAllEnemies();

            // Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public void GetAllEnemies_WhenNoEnemiesExist_ReturnsEmptyList()
        {
            // Arrange
            var enemies = new List<Enemy>();
            _mockRepository.Setup(repo => repo.GetAllEnemies()).Returns(enemies);

            // Act
            var result = _enemyService.GetAllEnemies();

            // Assert
            Assert.Empty(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(5)]
        public void GetAllEnemies_ReturnsCorrectNumberOfEnemies(int numberOfEnemies)
        {
            // Arrange
            var enemies = Enumerable.Range(1, numberOfEnemies)
                .Select(id => new Enemy
                {
                    Id = id,
                    Name = $"Enemy{id}",
                    Health = 100,
                    ImagePath = $"/images/enemy{id}.png"
                })
                .ToList();
            _mockRepository.Setup(repo => repo.GetAllEnemies()).Returns(enemies);

            // Act
            var result = _enemyService.GetAllEnemies();

            // Assert
            Assert.Equal(numberOfEnemies, result.Count);
        }

        // CreateEnemy Tests

        [Fact]
        public void CreateEnemy_ValidEnemyDTO_ReturnsEnemyDTOWithId()
        {
            // Arrange
            var enemyDTO = new EnemyDTO
            {
                Name = "Troll",
                Health = 300,
                ImagePath = "/images/troll.png"
            };
            Enemy savedEnemy = null;

            _mockRepository.Setup(repo => repo.AddEnemy(It.IsAny<Enemy>()))
                .Callback<Enemy>(enemy =>
                {
                    enemy.Id = 3; // Simulate database generated Id
                    savedEnemy = enemy;
                });

            // Act
            var result = _enemyService.CreateEnemy(enemyDTO);

            // Assert
            Assert.Equal(3, result.Id);
        }

        [Fact]
        public void CreateEnemy_ValidEnemyDTO_CallsAddEnemyOnce()
        {
            // Arrange
            var enemyDTO = new EnemyDTO
            {
                Name = "Troll",
                Health = 300,
                ImagePath = "/images/troll.png"
            };

            _mockRepository.Setup(repo => repo.AddEnemy(It.IsAny<Enemy>()));

            // Act
            _enemyService.CreateEnemy(enemyDTO);

            // Assert
            _mockRepository.Verify(repo => repo.AddEnemy(It.IsAny<Enemy>()), Times.Once);
        }

        [Fact]
        public void CreateEnemy_ValidEnemyDTO_CreatesEnemyWithCorrectProperties()
        {
            // Arrange
            var enemyDTO = new EnemyDTO
            {
                Name = "Troll",
                Health = 300,
                ImagePath = "/images/troll.png"
            };
            Enemy savedEnemy = null;

            _mockRepository.Setup(repo => repo.AddEnemy(It.IsAny<Enemy>()))
                .Callback<Enemy>(enemy =>
                {
                    enemy.Id = 3; // Simulate database generated Id
                    savedEnemy = enemy;
                });

            // Act
            _enemyService.CreateEnemy(enemyDTO);

            // Assert
            Assert.Equal("Troll", savedEnemy.Name);
            Assert.Equal(300, savedEnemy.Health);
            Assert.Equal("/images/troll.png", savedEnemy.ImagePath);
        }

        // UpdateEnemy Tests

        [Fact]
        public void UpdateEnemy_NonExistingEnemyDTO_ReturnsNull()
        {
            // Arrange
            var enemyDTO = new EnemyDTO
            {
                Id = 2,
                Name = "Orc",
                Health = 200,
                ImagePath = "/images/orc.png"
            };
            _mockRepository.Setup(repo => repo.GetEnemyById(enemyDTO.Id)).Returns((Enemy)null);

            // Act
            var result = _enemyService.UpdateEnemy(enemyDTO);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void UpdateEnemy_ExistingEnemyDTO_UpdatesEnemyPropertiesCorrectly()
        {
            // Arrange
            var enemyDTO = new EnemyDTO
            {
                Id = 1,
                Name = "Goblin King",
                Health = 500,
                ImagePath = "/images/goblin_king.png"
            };
            var existingEnemy = new Enemy
            {
                Id = 1,
                Name = "Goblin",
                Health = 100,
                ImagePath = "/images/goblin.png"
            };
            _mockRepository.Setup(repo => repo.GetEnemyById(enemyDTO.Id)).Returns(existingEnemy);

            // Act
            _enemyService.UpdateEnemy(enemyDTO);

            // Assert
            Assert.Equal("Goblin King", existingEnemy.Name);
            Assert.Equal(500, existingEnemy.Health);
            Assert.Equal("/images/goblin_king.png", existingEnemy.ImagePath);
        }

        [Fact]
        public void UpdateEnemy_ExistingEnemyDTO_CallsUpdateEnemyOnce()
        {
            // Arrange
            var enemyDTO = new EnemyDTO
            {
                Id = 1,
                Name = "Goblin King",
                Health = 500,
                ImagePath = "/images/goblin_king.png"
            };
            var existingEnemy = new Enemy { Id = 1 };
            _mockRepository.Setup(repo => repo.GetEnemyById(enemyDTO.Id)).Returns(existingEnemy);
            _mockRepository.Setup(repo => repo.UpdateEnemy(existingEnemy));

            // Act
            _enemyService.UpdateEnemy(enemyDTO);

            // Assert
            _mockRepository.Verify(repo => repo.UpdateEnemy(existingEnemy), Times.Once);
        }

        // DeleteEnemy Tests

        [Fact]
        public void DeleteEnemy_ExistingId_CallsDeleteEnemyOnce()
        {
            // Arrange
            int enemyId = 1;
            var existingEnemy = new Enemy { Id = enemyId };
            _mockRepository.Setup(repo => repo.GetEnemyById(enemyId)).Returns(existingEnemy);
            _mockRepository.Setup(repo => repo.DeleteEnemy(existingEnemy));

            // Act
            _enemyService.DeleteEnemy(enemyId);

            // Assert
            _mockRepository.Verify(repo => repo.DeleteEnemy(existingEnemy), Times.Once);
        }

        [Fact]
        public void DeleteEnemy_NonExistingId_DoesNotCallDeleteEnemy()
        {
            // Arrange
            int enemyId = 1;
            _mockRepository.Setup(repo => repo.GetEnemyById(enemyId)).Returns((Enemy)null);

            // Act
            _enemyService.DeleteEnemy(enemyId);

            // Assert
            _mockRepository.Verify(repo => repo.DeleteEnemy(It.IsAny<Enemy>()), Times.Never);
        }
    }
}
