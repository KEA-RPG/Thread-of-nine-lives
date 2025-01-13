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
            // Wait for the page to load by checking the document ready state
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").ToString() == "complete");

            // Open comments on first deck
            IWebElement firstViewCommentsButton = _driver.FindElement(By.XPath("(//button[contains(text(), 'View Comments')])[1]"));
            firstViewCommentsButton.Click();

            // Handle modal
            wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            IWebElement modalContent = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".chakra-modal__content")));
            IWebElement modalBody = modalContent.FindElement(By.CssSelector(".chakra-modal__body"));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollTop = arguments[0].scrollHeight;", modalBody);

            // Verify comment is visible
            IWebElement commentElement = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//header[contains(@class, 'chakra-modal__header') and contains(text(), 'Comments for')]"))); 
            Assert.True(commentElement.Displayed, "The comment title is not visible on the deck.");

            _driver.Close();
        }
    }
}
