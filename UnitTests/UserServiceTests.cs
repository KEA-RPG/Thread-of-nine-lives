using Backend.Services;
using Backend.Repositories;
using Backend.Models;
using Domain.Entities;
using Moq;
using Xunit;

namespace Backend.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _userService = new UserService(_mockUserRepository.Object);
        }

        [Fact]
        public void CreateUser_ShouldHashPassword_WhenUserIsCreated()
        {
            // Arrange
            var credentials = new Credentials { Username = "testuser", Password = "testpassword" };
            User capturedUser = null;

            _mockUserRepository
                .Setup(repo => repo.CreateUser(It.IsAny<User>()))
                .Callback<User>(user => capturedUser = user);

            // Act
            _userService.CreateUser(credentials);

            // 
            Assert.NotEqual("testpassword", capturedUser.PasswordHash);
        }

        [Fact]
        public void CreateUser_ShouldSetRoleToPlayer_WhenUserIsCreated()
        {
            // Arrange
            var credentials = new Credentials { Username = "testuser", Password = "testpassword" };

            // Act
            _userService.CreateUser(credentials);

            // Assert
            _mockUserRepository.Verify(repo => repo.CreateUser(It.Is<User>(
                user => user.Role == "Player"
            )), Times.Once);
        }

        [Fact]
        public void ValidateUserCredentials_ShouldReturnTrue_WhenCredentialsAreValid()
        {
            // Arrange
            var username = "testuser";
            var password = "testpassword";
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User { Username = username, PasswordHash = passwordHash, Role = "Player" };
            _mockUserRepository.Setup(repo => repo.GetUserByUsername(username)).Returns(user);

            // Act
            var result = _userService.ValidateUserCredentials(username, password);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidateUserCredentials_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            // Arrange
            var username = "nonexistentuser";
            _mockUserRepository.Setup(repo => repo.GetUserByUsername(username)).Returns((User)null);

            // Act
            var result = _userService.ValidateUserCredentials(username, "password");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateUserCredentials_ShouldReturnFalse_WhenPasswordIsIncorrect()
        {
            // Arrange
            var username = "testuser";
            var passwordHash = BCrypt.Net.BCrypt.HashPassword("correctpassword");

            var user = new User { Username = username, PasswordHash = passwordHash, Role = "Player" };
            _mockUserRepository.Setup(repo => repo.GetUserByUsername(username)).Returns(user);

            // Act
            var result = _userService.ValidateUserCredentials(username, "wrongpassword");

            // Assert
            Assert.False(result);
        }
    }
}
