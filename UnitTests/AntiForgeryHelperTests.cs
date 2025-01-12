using System;
using System.Threading.Tasks;
using Backend.SecurityLogic;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Backend.Tests
{
    public class AntiForgeryHelperTests
    {
        [Fact]
        public async Task ValidateAntiForgeryToken_CallsValidateRequestAsync()
        {
            // Arrange
            var mockAntiforgery = new Mock<IAntiforgery>();
            mockAntiforgery
                .Setup(x => x.ValidateRequestAsync(It.IsAny<HttpContext>()))
                .Returns(Task.CompletedTask);

            var httpContext = CreateHttpContextWithService(mockAntiforgery.Object);

            // Act
            await AntiForgeryHelper.ValidateAntiForgeryToken(httpContext);

            // Assert
            mockAntiforgery.Verify(
                x => x.ValidateRequestAsync(httpContext),
                Times.Once
            );
        }

        [Fact]
        public async Task ValidateAntiForgeryToken_DoesNotThrow_WhenValidationSucceeds()
        {
            // Arrange
            var mockAntiforgery = new Mock<IAntiforgery>();
            mockAntiforgery
                .Setup(x => x.ValidateRequestAsync(It.IsAny<HttpContext>()))
                .Returns(Task.CompletedTask);

            var httpContext = CreateHttpContextWithService(mockAntiforgery.Object);
            
            
            // Act & Assert
            var exception = await Record.ExceptionAsync(
                () => AntiForgeryHelper.ValidateAntiForgeryToken(httpContext)
            );

            Assert.Null(exception);
        }

        [Fact]
        public async Task ValidateAntiForgeryToken_RethrowsAntiforgeryValidationException()
        {
            // Arrange
            var mockAntiforgery = new Mock<IAntiforgery>();
            mockAntiforgery
                .Setup(x => x.ValidateRequestAsync(It.IsAny<HttpContext>()))
                .Throws(new AntiforgeryValidationException("Invalid token"));

            var httpContext = CreateHttpContextWithService(mockAntiforgery.Object);

            // Act & Assert
            await Assert.ThrowsAsync<AntiforgeryValidationException>(
                () => AntiForgeryHelper.ValidateAntiForgeryToken(httpContext)
            );
        }

        private static HttpContext CreateHttpContextWithService(IAntiforgery antiforgery)
        {
            var services = new ServiceCollection();
            services.AddSingleton(antiforgery);

            var serviceProvider = services.BuildServiceProvider();
            var context = new DefaultHttpContext
            {
                RequestServices = serviceProvider
            };
            return context;
        }
    }
}
