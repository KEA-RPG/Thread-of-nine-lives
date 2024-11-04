using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace ThreadOfNineLives.IntegrationTests
{
    public class IntegrationTestBase : IClassFixture<WebApplicationFactory<Program>>
    {
        protected readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

        public IntegrationTestBase(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    // Mock necessary services for testing
                    services.AddScoped<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            });
            _client = _factory.CreateClient();
        }

        protected string GenerateJwtToken(string role, DateTime? expiration = null)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("UngnjU6otFg8IumrmGgl-MbWUUc9wMk0HR37M-VYs6s="));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "testuser"),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: "threadgame",
                audience: "threadgame",
                claims: claims,
                expires: expiration ?? DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class AuthenticationTests : IntegrationTestBase
    {
        public AuthenticationTests(WebApplicationFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Access_ProtectedEndpoint_WithoutToken_ShouldReturnUnauthorized()
        {
            // Arrange
            var response = await _client.GetAsync("/protected-endpoint");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Access_PlayerEndpoint_WithPlayerRole_ShouldReturnSuccess()
        {
            // Arrange
            var token = GenerateJwtToken("Player");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.GetAsync("/player-only-endpoint");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Access_AdminEndpoint_WithPlayerRole_ShouldReturnForbidden()
        {
            // Arrange
            var token = GenerateJwtToken("Player");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.GetAsync("/admin-only-endpoint");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Access_AdminEndpoint_WithAdminRole_ShouldReturnSuccess()
        {
            // Arrange
            var token = GenerateJwtToken("Admin");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.GetAsync("/admin-only-endpoint");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Access_ProtectedEndpoint_WithExpiredToken_ShouldReturnUnauthorized()
        {
            // Arrange
            var token = GenerateJwtToken("Player", DateTime.UtcNow.AddMinutes(-10)); // Expired 10 minutes ago
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.GetAsync("/protected-endpoint");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }

    public class FakePolicyEvaluator : IPolicyEvaluator
    {
        public Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
        {
            var authenticationTicket = new AuthenticationTicket(new ClaimsPrincipal(), "FakeScheme");
            return Task.FromResult(AuthenticateResult.Success(authenticationTicket));
        }

        public Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy policy, AuthenticateResult authenticationResult, HttpContext context, object resource)
        {
            return Task.FromResult(PolicyAuthorizationResult.Success());
        }
    }
}
