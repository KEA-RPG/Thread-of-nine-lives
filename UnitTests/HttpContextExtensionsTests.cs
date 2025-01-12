using System.Security.Claims;
using Backend.Extensions;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace UnitTests
{
    public class HttpContextExtensionsTests
    {
        [Fact]
        public void GetUserName_ReturnsCorrectUserName_WhenClaimExists()
        {
            // Arrange
            var username = "testuser";
            var claims = new[] { new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", username) };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);

            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(c => c.User).Returns(principal);

            // Act
            var result = contextMock.Object.GetUserName();

            // Assert
            Assert.Equal(username, result);
        }

        [Fact]
        public void GetUserName_ReturnsNull_WhenClaimDoesNotExist()
        {
            // Arrange
            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(c => c.User).Returns(new ClaimsPrincipal());

            // Act
            var result = contextMock.Object.GetUserName();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetUserRole_ReturnsCorrectUserRole_WhenClaimExists()
        {
            // Arrange
            var role = "Admin";
            var claims = new[] { new Claim(ClaimTypes.Role, role) };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);

            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(c => c.User).Returns(principal);

            // Act
            var result = contextMock.Object.GetUserRole();

            // Assert
            Assert.Equal(role, result);
        }

        [Fact]
        public void GetUserRole_ReturnsNull_WhenClaimDoesNotExist()
        {
            // Arrange
            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(c => c.User).Returns(new ClaimsPrincipal());

            // Act
            var result = contextMock.Object.GetUserRole();

            // Assert
            Assert.Null(result);
        }
    }
}
