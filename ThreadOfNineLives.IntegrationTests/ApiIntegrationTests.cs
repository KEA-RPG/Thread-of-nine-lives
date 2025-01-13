using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Reflection.Metadata.Ecma335;

namespace ThreadOfNineLives.IntegrationTests
{
    public class ApiIntegrationTests : IDisposable
    {
        private readonly HttpClient _httpClient;

        public ApiIntegrationTests()
        {
            // Initialize HttpClient
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://loremflickr.com")
            };
        }

        [Fact]
        public async Task Get_Endpoint_ReturnsSuccessAndExpectedData()
        {
            // Arrange
            var endpoint = "/1280/720";

            // Act
            var response = await _httpClient.GetAsync(endpoint);

            // Assert
            Assert.True(response.IsSuccessStatusCode, $"Expected success but got {response.StatusCode}");

            var responseData = await response.Content.ReadAsStringAsync();
            Assert.False(string.IsNullOrEmpty(responseData), "Response content should not be empty");
        }

        [Theory]
        [InlineData(200,316)]
        [InlineData(1280,720)]
        [InlineData(800,1264)]
        public async Task Get_Image_ReturnsCorrectDimensions(int width, int height)
        {
            // Arrange
            var endpoint = "/"+ width + "/"+ height;

            // Act
            var response = await _httpClient.GetAsync(endpoint);

            // Assert
 
            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                var image = await Image.LoadAsync<Rgba32>(stream);
                Assert.Equal(width, image.Width);
                Assert.Equal(height, image.Height);
            }
        }

        [Fact]
        public async Task Get_Image_Is_Jpg()
        {
            // Arrange
            var endpoint = "/200/316";

            // Act
            var response = await _httpClient.GetAsync(endpoint);

            // Assert
            Assert.True(response.Content.Headers.ContentType.MediaType == "image/jpeg", "Expected image/jpeg content type");

        }

        [Fact]
        public async Task Get_Image_Returns_Success_Status_Code()
        {
            // Arrange
            var endpoint = "/200/316";

            // Act
            var response = await _httpClient.GetAsync(endpoint);

            // Assert
            Assert.True(response.IsSuccessStatusCode, $"Expected success but got {response.StatusCode}");
        }

        // Test for 404
        [Fact]
        public async Task Given_Incorrect_EndPoint_Returns_Error()
        {
            // Arrange
            var endpoint = "srgsgsgvsgvsvsg";

            // Act
            var response = await _httpClient.GetAsync(endpoint);

            // Assert
            Assert.False(response.IsSuccessStatusCode, $"Expected 404 but got {response.StatusCode}");
        }

        // Cleanup
        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}