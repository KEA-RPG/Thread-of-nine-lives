using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ThreadOfNineLives.E2ETest
{
    public class TestE2E
    {
        private readonly IWebDriver _driver;

        public TestE2E()
        {
            _driver = new ChromeDriver();
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
    }
}