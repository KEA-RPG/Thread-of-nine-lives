using Xunit;
using Moq;
using Backend.Services;
using Domain.Entities;
using Domain.DTOs;
using System.Collections.Generic;
using System.Linq;
using Backend.Repositories.Interfaces;

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
            var enemy = CreateEnemyDTO();

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
            var enemy = new EnemyDTO
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
            _mockRepository.Setup(repo => repo.GetEnemyById(enemyId)).Returns((EnemyDTO)null);

            // Act
            var result = _enemyService.GetEnemyById(enemyId);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void GetEnemyById_NonExistingIds_ReturnsNull(int enemyId)
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetEnemyById(enemyId)).Returns((EnemyDTO)null);

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
            var enemies = new List<EnemyDTO>
            {
                new EnemyDTO { Id = 1, Name = "Goblin", Health = 100, ImagePath = "/images/goblin.png" },
                new EnemyDTO { Id = 2, Name = "Orc", Health = 200, ImagePath = "/images/orc.png" }
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
            var enemies = new List<EnemyDTO>();
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
                .Select(id => new EnemyDTO
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

            var expectedEnemy = new EnemyDTO
            {
                Id = 3, // Simulate the database assigned Id
                Name = "Troll",
                Health = 300,
                ImagePath = "/images/troll.png"
            };

            _mockRepository.Setup(repo => repo.AddEnemy(It.IsAny<EnemyDTO>()))
                .Returns(expectedEnemy);

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

            _mockRepository.Setup(repo => repo.AddEnemy(It.IsAny<EnemyDTO>()));

            // Act
            _enemyService.CreateEnemy(enemyDTO);

            // Assert
            _mockRepository.Verify(repo => repo.AddEnemy(It.IsAny<EnemyDTO>()), Times.Once);
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

            var expectedEnemy = new EnemyDTO
            {
                Id = 3, // Simulate the database assigned Id
                Name = "Troll",
                Health = 300,
                ImagePath = "/images/troll.png"
            };

            _mockRepository.Setup(repo => repo.AddEnemy(It.IsAny<EnemyDTO>()))
                .Returns(expectedEnemy);

            // Act
            var result = _enemyService.CreateEnemy(enemyDTO);

            // Assert
            Assert.Equal("Troll", result.Name);
            Assert.Equal(300, result.Health);
            Assert.Equal("/images/troll.png", result.ImagePath);
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
            _mockRepository.Setup(repo => repo.GetEnemyById(enemyDTO.Id)).Returns((EnemyDTO)null);

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
            var existingEnemy = new EnemyDTO
            {
                Id = 1,
                Name = "Goblin",
                Health = 100,
                ImagePath = "/images/goblin.png"
            };
            _mockRepository.Setup(repo => repo.GetEnemyById(enemyDTO.Id)).Returns(existingEnemy);

            // Act
            var result = _enemyService.UpdateEnemy(enemyDTO);

            // Assert
            Assert.Equal("Goblin King", result.Name);
            Assert.Equal(500, result.Health);
            Assert.Equal("/images/goblin_king.png", result.ImagePath);
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
            var existingEnemy = CreateEnemyDTO();
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
            var existingEnemy = CreateEnemyDTO();
            existingEnemy.Id = 1;
            _mockRepository.Setup(repo => repo.GetEnemyById(existingEnemy.Id)).Returns(existingEnemy);
            _mockRepository.Setup(repo => repo.DeleteEnemy(existingEnemy));

            // Act
            _enemyService.DeleteEnemy(existingEnemy.Id);

            // Assert
            _mockRepository.Verify(repo => repo.DeleteEnemy(existingEnemy), Times.Once);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void DeleteEnemy_NonExistingId_DoesNotCallDeleteEnemy(int id)
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetEnemyById(id)).Returns((EnemyDTO)null);

            // Act
            _enemyService.DeleteEnemy(id);

            // Assert
            _mockRepository.Verify(repo => repo.DeleteEnemy(It.IsAny<EnemyDTO>()), Times.Never);
        }

          private EnemyDTO CreateEnemyDTO()
        {
            return new EnemyDTO()
            {
                Health = 100,
                ImagePath = "test",
                Name = "Test enemy"
            };
        }
    }
}
