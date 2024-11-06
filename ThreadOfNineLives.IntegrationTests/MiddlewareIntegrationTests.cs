using Moq;
using Backend.Repositories;
using Backend.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Newtonsoft.Json;
using System.Linq;

namespace ThreadOfNineLives.IntegrationTests
{
    public class MiddlewareIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public MiddlewareIntegrationTests(WebApplicationFactory<Program> factory)
        {
            // Setup the factory with mocked services
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove the existing IUserRepository registration if any
                    var userDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IUserRepository));
                    if (userDescriptor != null)
                    {
                        services.Remove(userDescriptor);
                    }

                    // Create a mock IUserRepository
                    var mockUserRepository = new Mock<IUserRepository>();

                    // Setup mock behavior for the admin user
                    mockUserRepository.Setup(repo => repo.GetUserByUsername("adminUsername"))
                        .Returns(new Domain.Entities.User
                        {
                            Username = "adminUsername",
                            PasswordHash = BCrypt.Net.BCrypt.HashPassword("adminPassword"),
                            Role = "Admin"
                        });

                    // Setup mock behavior for the player user
                    mockUserRepository.Setup(repo => repo.GetUserByUsername("playerUsername"))
                        .Returns(new Domain.Entities.User
                        {
                            Username = "playerUsername",
                            PasswordHash = BCrypt.Net.BCrypt.HashPassword("playerPassword"),
                            Role = "Player"
                        });

                    // Register the mock repository
                    services.AddScoped(_ => mockUserRepository.Object);

                    // Remove the existing IEnemyRepository registration if any
                    var enemyDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IEnemyRepository));
                    if (enemyDescriptor != null)
                    {
                        services.Remove(enemyDescriptor);
                    }

                    // Create a mock IEnemyRepository
                    var mockEnemyRepository = new Mock<IEnemyRepository>();

                    // Setup mock behavior for an enemy
                    mockEnemyRepository.Setup(repo => repo.GetEnemyById(1))
                        .Returns(new Domain.Entities.Enemy
                        {
                            Id = 1,
                            Name = "Test Enemy",
                            Health = 100
                        });

                    // Setup mock behavior for getting all enemies
                    mockEnemyRepository.Setup(repo => repo.GetAllEnemies())
                        .Returns(new List<Domain.Entities.Enemy>
                        {
                    new Domain.Entities.Enemy
                    {
                        Id = 1,
                        Name = "Test Enemy",
                        Health = 100
                    }
                        });

                    // Register the mock repository
                    services.AddScoped(_ => mockEnemyRepository.Object);
                });
            });
        }


        [Fact]
        public async Task Authorize_AdminRole_ShouldReturnSuccessForAdminDeleteEndpoint()
        {
            // Arrange: Create a client
            var client = _factory.CreateClient();

            // Prepare login credentials
            var credentials = new { Username = "adminUsername", Password = "adminPassword" };
            var content = new StringContent(JsonConvert.SerializeObject(credentials), Encoding.UTF8, "application/json");

            // Send login request to get the JWT token
            var response = await client.PostAsync("/auth/login", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode(); // Throws an exception on failure

            var authResponse = JsonConvert.DeserializeObject<AuthResponse>(responseContent);
            if (authResponse == null || string.IsNullOrEmpty(authResponse.Token))
            {
                throw new Exception("Failed to retrieve the token from the authentication response.");
            }

            var token = authResponse.Token;

            // Add the token to the Authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act: Access the admin-protected delete endpoint
            var protectedResponse = await client.DeleteAsync("/enemies/1");
            var protectedResponseContent = await protectedResponse.Content.ReadAsStringAsync();

            // Assert: Expecting OK
            Assert.Equal(HttpStatusCode.OK, protectedResponse.StatusCode);
        }

        [Fact]
        public async Task Authorize_PlayerRole_ShouldReturnSuccessForPlayerGetEndpoint()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Prepare login credentials
            var credentials = new { Username = "playerUsername", Password = "playerPassword" };
            var content = new StringContent(JsonConvert.SerializeObject(credentials), Encoding.UTF8, "application/json");

            // Send login request to get the JWT token
            var response = await client.PostAsync("/auth/login", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode(); // Throws an exception on failure

            var authResponse = JsonConvert.DeserializeObject<AuthResponse>(responseContent);
            if (authResponse == null || string.IsNullOrEmpty(authResponse.Token))
            {
                throw new Exception("Failed to retrieve the token from the authentication response.");
            }

            var token = authResponse.Token;

            // Add the token to the Authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act: Access the player-protected get endpoint
            var protectedResponse = await client.GetAsync("/enemies");
            var protectedResponseContent = await protectedResponse.Content.ReadAsStringAsync();

            // Assert: Expecting OK
            Assert.Equal(HttpStatusCode.OK, protectedResponse.StatusCode);
        }

        [Fact]
        public async Task Authorize_InvalidRole_ShouldReturnForbiddenForAdminDeleteEndpoint()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Prepare login credentials
            var credentials = new { Username = "playerUsername", Password = "playerPassword" };
            var content = new StringContent(JsonConvert.SerializeObject(credentials), Encoding.UTF8, "application/json");

            // Send login request to get the JWT token
            var response = await client.PostAsync("/auth/login", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode(); // Throws an exception on failure

            var authResponse = JsonConvert.DeserializeObject<AuthResponse>(responseContent);
            if (authResponse == null || string.IsNullOrEmpty(authResponse.Token))
            {
                throw new Exception("Failed to retrieve the token from the authentication response.");
            }

            var token = authResponse.Token;

            // Add the token to the Authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act: Try to access the admin-protected delete endpoint
            var protectedResponse = await client.DeleteAsync("/enemies/1");
            var protectedResponseContent = await protectedResponse.Content.ReadAsStringAsync();

            // Assert: Expecting Forbidden
            Assert.Equal(HttpStatusCode.Forbidden, protectedResponse.StatusCode);
        }

        [Fact]
        public async Task NoAuthorizationHeader_ShouldReturnUnauthorizedForAdminDeleteEndpoint()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act: Try to access the admin-protected delete endpoint without a token
            var response = await client.DeleteAsync("/enemies/1");
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert: Expecting Unauthorized
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        public class AuthResponse
        {
            public string Token { get; set; }
        }

        public class EnemyResponse
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Health { get; set; }
        }
    }
}