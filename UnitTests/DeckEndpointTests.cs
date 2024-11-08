using Xunit;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;
using System.Collections.Generic;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Backend.Controllers;
using Domain.DTOs;
using System.IdentityModel.Tokens.Jwt;


namespace Backend.Tests.Controllers
{

/*    public static class JwtClaimNames
    {
        public const string Sub = "sub";
        public const string Jti = "jti";
        public const string Role = "role";
        // Add other claims as necessary
    }

    public class DeckControllerEndpointTests
    {
        [Fact]
        public void HandleGetDecks_ReturnsOk_WhenUserIsAuthenticated()
        {
            // Arrange
            var mockDeckService = new Mock<IDeckService>();
            var mockContext = new DefaultHttpContext();
            string expectedUserName = "testUser";

            // Use "sub" claim to match the logic in HandleGetDecks
            var claims = new List<Claim> { new Claim(JwtClaimNames.Sub, expectedUserName) };
            mockContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims));

            mockDeckService.Setup(s => s.GetUserDecks(expectedUserName))
                           .Returns(new List<DeckDTO> { new DeckDTO { Name = "Deck1" }, new DeckDTO { Name = "Deck2" } });

            // Act
            var result = DeckController.HandleGetDecks(mockDeckService.Object, mockContext);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.Ok<List<DeckDTO>>>(result);

            // Optional: Verify the actual content
            var okResult = result as Microsoft.AspNetCore.Http.HttpResults.Ok<List<DeckDTO>>;
            Assert.NotNull(okResult);
            Assert.Equal(2, okResult.Value.Count);
            Assert.Equal("Deck1", okResult.Value[0].Name);
            Assert.Equal("Deck2", okResult.Value[1].Name);
        }

        [Fact]
        public void HandleGetDecks_ReturnsUnauthorized_WhenUserIsNotAuthenticated()
        {
            // Arrange
            var mockDeckService = new Mock<IDeckService>();
            var mockContext = new DefaultHttpContext();

            // Act
            var result = DeckController.HandleGetDecks(mockDeckService.Object, mockContext);

            // Assert
            Assert.IsType<UnauthorizedHttpResult>(result);
        }
    }*/
}
