using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace EndToEndTests
{
    public class PlayerEndToEndTests
    {
        private readonly IWebDriver _driver;

        public PlayerEndToEndTests()
        {
            _driver = new ChromeDriver();

            _driver.Navigate().GoToUrl("http://localhost:5173/");

            _driver.Manage().Window.Size = new System.Drawing.Size(949, 743);

            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(3000);
        }

        [Fact]
        public void Sign_up()
        {
            IWebElement linkElement = _driver.FindElement(By.XPath("//a[text()='here']"));
            linkElement.Click();

            IWebElement usernameInputElement = _driver.FindElement(By.CssSelector("input[placeholder='Username']"));
            usernameInputElement.Clear();
            usernameInputElement.SendKeys("testuser");

            IWebElement passwordInputElement = _driver.FindElement(By.CssSelector("input[placeholder='Password']"));
            passwordInputElement.Clear();
            passwordInputElement.SendKeys("testpassword");

            IWebElement rePasswordInputElement = _driver.FindElement(By.CssSelector("input[placeholder='Repeat Password']"));
            rePasswordInputElement.Click();
            rePasswordInputElement.SendKeys("testpassword");

            IWebElement signUpButtonElement = _driver.FindElement(By.XPath("//button[contains(text(),'Sign up')]"));
            signUpButtonElement.Click();

            _driver.Close();
        }

        [Fact]
        public void Sign_in()
        {
            IWebElement usernameInputElement = _driver.FindElement(By.CssSelector("input[placeholder='Username']"));
            usernameInputElement.Clear();
            usernameInputElement.SendKeys("testuser");
            IWebElement passwordInputElement = _driver.FindElement(By.CssSelector("input[placeholder='Password']"));
            passwordInputElement.Clear();
            passwordInputElement.SendKeys("testpassword");
            IWebElement signInButtonElement = _driver.FindElement(By.XPath("//button[contains(text(),'Sign in')]"));
            signInButtonElement.Click();

            IWebElement element = _driver.FindElement(By.XPath("//p[text()='Fight!']"));
            string text = element.Text;

            Assert.Equal("Fight!", text);
            _driver.Close();
        }

        public void Sign_out()
        {
            // Sign in
            IWebElement usernameInputElement = _driver.FindElement(By.CssSelector("input[placeholder='Username']"));
            usernameInputElement.Clear();
            usernameInputElement.SendKeys("testuser");
            IWebElement passwordInputElement = _driver.FindElement(By.CssSelector("input[placeholder='Password']"));
            passwordInputElement.Clear();
            passwordInputElement.SendKeys("testpassword");
            IWebElement signInButtonElement = _driver.FindElement(By.XPath("//button[contains(text(),'Sign in')]"));
            signInButtonElement.Click();

            // Sign out
            IWebElement signOutButtonElement = _driver.FindElement(By.XPath("//button[contains(text(),'Logout')]"));
            signOutButtonElement.Click();
            IWebElement element = _driver.FindElement(By.XPath("//p[text()='Log in']"));
            string text = element.Text;
            Assert.Equal("Log in", text);

            _driver.Close();
        }

        [Fact]
        public void Create_update_delete_deck()
        {
            // Sign in
            IWebElement usernameInputElement = _driver.FindElement(By.CssSelector("input[placeholder='Username']"));
            usernameInputElement.Clear();
            usernameInputElement.SendKeys("testuser");
            IWebElement passwordInputElement = _driver.FindElement(By.CssSelector("input[placeholder='Password']"));
            passwordInputElement.Clear();
            passwordInputElement.SendKeys("testpassword");
            IWebElement signInButtonElement = _driver.FindElement(By.XPath("//button[contains(text(),'Sign in')]"));
            signInButtonElement.Click();

            // Create deck
            IWebElement decksButtonElement = _driver.FindElement(By.XPath("//p[text()='Decks']"));
            decksButtonElement.Click();
            IWebElement newButtonElement = _driver.FindElement(By.XPath("//button[contains(text(),'New')]"));
            newButtonElement.Click();
            IWebElement playingCard = _driver.FindElement(By.XPath("//h4[text()='Guidance']/ancestor::div[@tabindex='0']"));
            int numberOfCardsToAdd = 3;
            for (int i = 0; i < numberOfCardsToAdd; i++)
            {
                playingCard.Click();
            }
            IWebElement deckCard = _driver.FindElement(By.XPath("//p[text()='Guidance']"));
            deckCard.Click();
            IWebElement numberElement = _driver.FindElement(By.XPath("//div[contains(@class, 'chakra-stack') and descendant::p[text()='Guidance']]//p[contains(text(), '(')]"));
            string textContent = numberElement.Text;
            int value = int.Parse(textContent.Trim('(', ')'));
            Assert.Equal(2, value);
            IWebElement clearDeckButton = _driver.FindElement(By.XPath("//button[text()='Clear Deck']"));
            clearDeckButton.Click();
            numberOfCardsToAdd = 2;
            for (int i = 0; i < numberOfCardsToAdd; i++)
            {
                playingCard.Click();
            }
            IWebElement deckNameInput = _driver.FindElement(By.CssSelector("input[placeholder='Deck name']"));
            deckNameInput.SendKeys("testdeck");
            IWebElement checkboxControl = _driver.FindElement(By.CssSelector(".chakra-checkbox__control"));
            checkboxControl.Click();
            IWebElement saveDeckBtn = _driver.FindElement(By.XPath("//button[text()='Save deck']"));
            saveDeckBtn.Click();

            // Edit deck
            IWebElement editButtonElement = _driver.FindElement(By.XPath("//button[contains(text(),'Edit')]"));
            editButtonElement.Click();
            playingCard = _driver.FindElement(By.XPath("//h4[text()='Guidance']/ancestor::div[@tabindex='0']"));
            playingCard.Click();
            numberElement = _driver.FindElement(By.XPath("//div[contains(@class, 'chakra-stack') and descendant::p[text()='Guidance']]//p[contains(text(), '(')]"));
            textContent = numberElement.Text;
            int value2 = int.Parse(textContent.Trim('(', ')'));
            Assert.Equal(3, value2);
            IWebElement saveDeckBtn2 = _driver.FindElement(By.XPath("//button[text()='Save deck']"));
            saveDeckBtn2.Click();

            // Delete deck
            IWebElement deleteButtonElement = _driver.FindElement(By.XPath("//button[contains(text(),'Delete')]"));
            deleteButtonElement.Click();

            // Handle modal
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            IWebElement modal = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".chakra-modal__content")));
            IWebElement modalDeleteButton = modal.FindElement(By.XPath(".//button[contains(text(),'Delete')]"));
            modalDeleteButton.Click();

            // Wait until the "testdeck" element is no longer visible
            bool isDeleted = wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//div[contains(text(), 'testdeck')]")));
            Assert.True(isDeleted, "The deck 'testdeck' is still visible on the page.");

            _driver.Close();
        }

        [Fact]
        public void Create_comment()
        {
            // Sign in
            IWebElement usernameInputElement = _driver.FindElement(By.CssSelector("input[placeholder='Username']"));
            usernameInputElement.Clear();
            usernameInputElement.SendKeys("testuser");
            IWebElement passwordInputElement = _driver.FindElement(By.CssSelector("input[placeholder='Password']"));
            passwordInputElement.Clear();
            passwordInputElement.SendKeys("testpassword");
            IWebElement signInButtonElement = _driver.FindElement(By.XPath("//button[contains(text(),'Sign in')]"));
            signInButtonElement.Click();

            // Create comment
            IWebElement publicDecksButtonElement = _driver.FindElement(By.XPath("//p[text()='Public Decks']"));
            publicDecksButtonElement.Click();
            IWebElement viewCommentsButton = _driver.FindElement(By.XPath("//h2[contains(text(), 'Fire Deck')]/ancestor::div[contains(@class, 'chakra-card')]//button[contains(text(), 'View Comments')]"));
            viewCommentsButton.Click();

            // Handle modal
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            IWebElement modalContent = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".chakra-modal__content")));
            IWebElement modalBody = modalContent.FindElement(By.CssSelector(".chakra-modal__body"));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollTop = arguments[0].scrollHeight;", modalBody);
            IWebElement commentTextArea = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("textarea[placeholder='Enter your comment']")));
            commentTextArea.Clear();
            commentTextArea.SendKeys("This is a test comment");
            IWebElement submitButton = _driver.FindElement(By.CssSelector("button[type='submit'].chakra-button"));
            submitButton.Click();

            // Refresh page
            _driver.Navigate().Refresh();
            IWebElement viewCommentsButton2 = _driver.FindElement(By.XPath("//h2[contains(text(), 'Fire Deck')]/ancestor::div[contains(@class, 'chakra-card')]//button[contains(text(), 'View Comments')]"));
            viewCommentsButton2.Click();

            // Check if comment was created
            IWebElement commentElement = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//p[text()='This is a test comment']")));
            Assert.True(commentElement.Displayed, "The comment is not visible on the deck.");

            _driver.Close();
        }

        [Fact]
        public void Fight_win()
        {
            // Sign in
            IWebElement usernameInputElement = _driver.FindElement(By.CssSelector("input[placeholder='Username']"));
            usernameInputElement.Clear();
            usernameInputElement.SendKeys("testuser");
            IWebElement passwordInputElement = _driver.FindElement(By.CssSelector("input[placeholder='Password']"));
            passwordInputElement.Clear();
            passwordInputElement.SendKeys("testpassword");
            IWebElement signInButtonElement = _driver.FindElement(By.XPath("//button[contains(text(),'Sign in')]"));
            signInButtonElement.Click();

            // Create fight
            IWebElement fightButtonElement = _driver.FindElement(By.XPath("//p[text()='Fight!']"));
            fightButtonElement.Click();
            IWebElement sirMeowsalotDiv = _driver.FindElement(By.XPath("//div[text()='Troll']/following-sibling::div//button"));
            sirMeowsalotDiv.Click();
            IWebElement endTurnButton = _driver.FindElement(By.XPath("//button[text()='Attack']"));
            int numberOfClicks = 2;
            for (int i = 0; i < numberOfClicks; i++)
            {
                endTurnButton.Click();
            }
            IWebElement winMessage = _driver.FindElement(By.XPath("//p[text()='You Win!']"));
            Assert.True(winMessage.Displayed, "The 'You Win!' message is not visible on the page.");

            _driver.Close();
        }

        [Fact]
        public void Fight_lose()
        {
            // Sign in
            IWebElement usernameInputElement = _driver.FindElement(By.CssSelector("input[placeholder='Username']"));
            usernameInputElement.Clear();
            usernameInputElement.SendKeys("testuser");
            IWebElement passwordInputElement = _driver.FindElement(By.CssSelector("input[placeholder='Password']"));
            passwordInputElement.Clear();
            passwordInputElement.SendKeys("testpassword");
            IWebElement signInButtonElement = _driver.FindElement(By.XPath("//button[contains(text(),'Sign in')]"));
            signInButtonElement.Click();

            // Create fight
            IWebElement fightButtonElement = _driver.FindElement(By.XPath("//p[text()='Fight!']"));
            fightButtonElement.Click();
            IWebElement sirMeowsalotDiv = _driver.FindElement(By.XPath("//div[text()='Dragon']/following-sibling::div//button"));
            sirMeowsalotDiv.Click();
            IWebElement endTurnButton = _driver.FindElement(By.XPath("//button[text()='End Turn']"));
            int numberOfClicks = 5;
            for (int i = 0; i < numberOfClicks; i++)
            {
                endTurnButton.Click();
            }
            IWebElement winMessage = _driver.FindElement(By.XPath("//p[text()='You Lose!']"));
            Assert.True(winMessage.Displayed, "The 'You Win!' message is not visible on the page.");

            _driver.Close();
        }
    }
}
