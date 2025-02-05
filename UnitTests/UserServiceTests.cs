﻿using Backend.Services;
using Backend.Models;
using Domain.Entities;
using Domain.DTOs;
using Moq;
using Xunit;
using Backend.Repositories.Interfaces;

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
            var credentials = new Credentials
            {
                Username = "testuser",
                Password = "TestPass1!"
            };

            // Act
            _userService.CreateUser(credentials);

            // Assert
            _mockUserRepository.Verify(repo => repo.CreateUser(It.Is<UserDTO>(
                user => user.Password != "TestPass1!" // Ensure it was hashed
            )), Times.Once);
        }

        [Fact]
        public void CreateUser_ShouldSetRoleToPlayer_WhenUserIsCreated()
        {
            // Arrange
            var credentials = new Credentials
            {
                Username = "testuser",
                Password = "TestPass1!"
            };

            // Act
            _userService.CreateUser(credentials);

            // Assert
            _mockUserRepository.Verify(repo => repo.CreateUser(It.Is<UserDTO>(
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

            var user = new UserDTO { Username = username, Password = passwordHash, Role = "Player" };
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
            var password = "password";
            _mockUserRepository.Setup(repo => repo.GetUserByUsername(username)).Returns((UserDTO)null);

            // Act
            var result = _userService.ValidateUserCredentials(username, password);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateUserCredentials_ShouldReturnFalse_WhenPasswordIsIncorrect()
        {
            // Arrange
            var username = "testuser";
            var passwordHash = BCrypt.Net.BCrypt.HashPassword("correctpassword");

            var user = new UserDTO { Username = username, Password = passwordHash, Role = "Player" };
            _mockUserRepository.Setup(repo => repo.GetUserByUsername(username)).Returns(user);

            // Act
            var result = _userService.ValidateUserCredentials(username, "wrongpassword");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetUserByUsername_ShouldReturnUserDTO_WhenUserExists()
        {
            // Arrange
            var username = "testuser";
            var user = new UserDTO { Username = username, Password = "hashedpassword", Role = "Player" };
            _mockUserRepository.Setup(repo => repo.GetUserByUsername(username)).Returns(user);

            // Act
            var result = _userService.GetUserByUsername(username);

            // Assert
            Assert.Equal(username, result.Username);
            Assert.Equal("hashedpassword", result.Password);
            Assert.Equal("Player", result.Role);
        }

        [Fact]
        public void GetUserByUsername_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            var username = "nonexistentuser";
            _mockUserRepository.Setup(repo => repo.GetUserByUsername(username)).Returns((UserDTO)null);

            // Act
            var result = _userService.GetUserByUsername(username);

            // Assert
            Assert.Null(result);
        }
    }
}
