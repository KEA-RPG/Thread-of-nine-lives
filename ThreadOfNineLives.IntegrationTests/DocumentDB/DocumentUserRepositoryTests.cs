using Backend.Repositories.Document;
using Domain.DTOs;
using Infrastructure.Persistance;
using Infrastructure.Persistance.Document;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadOfNineLives.IntegrationTests.DocumentDB
{
    public class DocumentUserRepositoryTests :IDisposable
    {
        private readonly DocumentContext _context;
        private readonly MongoUserRepository _mongoUserRepository;
        private readonly DatabaseSnapshotHelper _snapshotHelper;
        public DocumentUserRepositoryTests()
        {
            _context = PersistanceConfiguration.GetDocumentContext(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
            _mongoUserRepository = new MongoUserRepository(_context);
            _snapshotHelper = new DatabaseSnapshotHelper(_context);

            _snapshotHelper.TakeSnapshot();
        }

        [Fact]
        public void AddEnemy_Assigns_Id_And_Retains_Values()
        {
            //Arrange
            var testUser = new UserDTO()
            {
               Password ="test",
               Role = "tester",
               Username = "test123"
            };

            //Act
            _mongoUserRepository.CreateUser(testUser);
            var data = _mongoUserRepository.GetUserByUsername(testUser.Username);

            //Assert
            Assert.NotNull(data);
            Assert.True(data.Id > 0);
            Assert.Equal(data.Password, testUser.Password);
            Assert.Equal(data.Role, testUser.Role);
            Assert.Equal(data.Username, testUser.Username);
        }

        [Fact]
        public void GetUserById_Returns_Data_Given_Valid_Id()
        {
            //Arrange
            var testUser = new UserDTO()
            {
                Password = "test",
                Role = "tester",
                Username = "test123"
            };
            _mongoUserRepository.CreateUser(testUser);

            var user = _mongoUserRepository.GetUserByUsername(testUser.Username);
            var userId = user.Id;

            //Act
            var data = _mongoUserRepository.GetUserById(userId);

            //Assert
            Assert.NotNull(data);
            Assert.True(data.Id > 0);
            Assert.Equal(data.Password, testUser.Password);
            Assert.Equal(data.Role, testUser.Role);
            Assert.Equal(data.Username, testUser.Username);
        }

        public void Dispose()
        {
            _snapshotHelper.RestoreSnapshot();
        }
    }
}
