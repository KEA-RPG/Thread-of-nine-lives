using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Drawing;

namespace ThreadOfNineLives.IntegrationTests
{

    public class ApiIntegrationTests
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

        [Fact]
        public async Task Get_Image_ReturnsCorrectDimensionsAndIsJpg()
        {
            // Arrange
            var endpoint = "/200/316"; // Replace with the API endpoint

            // Act
            var response = await _httpClient.GetAsync(endpoint);

            // Assert
            Assert.True(response.IsSuccessStatusCode, $"Expected success but got {response.StatusCode}");

            Assert.True(response.Content.Headers.ContentType.MediaType == "image/jpeg", "Expected image/jpeg content type");

            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                var image = Image.FromStream(stream);
                Assert.Equal(200, image.Width);
                Assert.Equal(316, image.Height);
            }
        }

        // Cleanup
        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}