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
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IUserRepository));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
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
                });
            });
        }

        [Fact]
        public async Task Authorize_AdminRole_ShouldReturnSuccess()
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

            // Act: Access a protected endpoint
            var protectedResponse = await client.GetAsync("/admintest");
            var protectedResponseContent = await protectedResponse.Content.ReadAsStringAsync();

            // Assert: Expecting OK
            Assert.Equal(HttpStatusCode.OK, protectedResponse.StatusCode);
        }

        [Fact]
        public async Task Authorize_PlayerRole_ShouldReturnSuccessForPlayerEndpoints()
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

            // Act
            var protectedResponse = await client.GetAsync("/playertest");
            var protectedResponseContent = await protectedResponse.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, protectedResponse.StatusCode);
        }

        [Fact]
        public async Task Authorize_InvalidRole_ShouldReturnForbidden()
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

            // Act
            var protectedResponse = await client.GetAsync("/cards");
            var protectedResponseContent = await protectedResponse.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, protectedResponse.StatusCode);
        }

        [Fact]
        public async Task NoAuthorizationHeader_ShouldReturnUnauthorized()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/admintest");
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        public class AuthResponse
        {
            public string Token { get; set; }
        }
    }
}
