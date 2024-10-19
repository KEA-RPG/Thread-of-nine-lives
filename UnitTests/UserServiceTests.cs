using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Backend.Repositories;
using Domain.Entities;
using Backend.Services;

namespace UnitTests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepositoryMock.Object);
        }

        [Fact]
        public void CreateUser_Should_Set_Player_Role()
        {
            // Arrange
            var newUser = new User { Username = "testuser", PasswordHash = "testpassword" };

            // Act
            _userService.CreateUser(newUser);

            // Assert
            _userRepositoryMock.Verify(repo => repo.CreateUser(It.IsAny<User>()), Times.Once);
            Assert.Equal("Player", newUser.Role);
        }

        [Fact]
        public void CreateUser_Should_Hash_Password()
        {
            // Arrange
            var newUser = new User { Username = "testuser", PasswordHash = "testpassword" };

            // Act
            _userService.CreateUser(newUser);

            // Assert
            Assert.NotEqual("testpassword", newUser.PasswordHash);
        }

        [Fact]
        public void ValidateUserCredentials_Should_Return_True_When_Password_Matches()
        {
            // Arrange
            var storedUser = new User { Username = "testuser", PasswordHash = BCrypt.Net.BCrypt.HashPassword("testpassword") };
            _userRepositoryMock.Setup(repo => repo.GetUserByUsername(It.IsAny<string>())).Returns(storedUser);

            // Act
            var result = _userService.ValidateUserCredentials("testuser", "testpassword");

            // Assert
            Assert.True(result);
        }
    }
}