using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace EndToEndTests
{
    public class AnonymousEndToEndTests
    {
        private readonly ChromeDriver _driver;

        public AnonymousEndToEndTests()
        {
            _driver = new ChromeDriver();

            _driver.Navigate().GoToUrl("http://localhost:5173/decks/public");

            _driver.Manage().Window.Size = new System.Drawing.Size(949, 743);

            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(3000);
        }

        [Fact]
        public void Anonymous_user_can_view_publicDecks()
        {
            // Open comments on deck
            IWebElement viewCommentsButton = _driver.FindElement(By.XPath("//h2[contains(text(), 'Fire Deck')]/ancestor::div[contains(@class, 'chakra-card')]//button[contains(text(), 'View Comments')]"));
            viewCommentsButton.Click();

            // Handle modal
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            IWebElement modalContent = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".chakra-modal__content")));
            IWebElement modalBody = modalContent.FindElement(By.CssSelector(".chakra-modal__body"));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollTop = arguments[0].scrollHeight;", modalBody);

            // Verify comment is visible
            IWebElement commentElement = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//p[text()='This is a test comment']")));
            Assert.True(commentElement.Displayed, "The comment is not visible on the deck.");

            _driver.Close();
        }
    }
}
