using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;

namespace ThreadOfNineLives.E2ETest
{
    public class TestE2E : IDisposable
    {
        private readonly IWebDriver _driver;

        public TestE2E()
        {
            var options = new ChromeOptions();

            var tempUserDataDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            options.AddArgument($"--user-data-dir={tempUserDataDir}");

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Docker")
            {
                Console.WriteLine("HIT THE BREAKPOINT");
                options.AddArgument("--headless");
                options.AddArgument("--no-sandbox");
                options.AddArgument("--disable-dev-shm-usage");
                options.BrowserVersion = "131.0.6778.139";
            }
            _driver = new ChromeDriver(options);
            _driver.Navigate().GoToUrl("http://localhost:5173/login");
            _driver.Manage().Window.Size = new System.Drawing.Size(949, 743);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(3000);
           
        }
        [Fact]
        public void Test1()
        {
            _driver.Navigate().GoToUrl("http://localhost:5173/decks/public");
            var submitButton = _driver.FindElement(By.CssSelector("button.chakra-button"));
            submitButton.Click();

        }
        public void Dispose()
        {
            _driver.Quit();
        }
    }
}