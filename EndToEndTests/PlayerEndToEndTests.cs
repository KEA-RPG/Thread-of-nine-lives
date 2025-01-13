using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using Infrastructure.Persistance;
using Infrastructure.Persistance.Relational;

namespace EndToEndTests
{
    public class PlayerEndToEndTests : IDisposable
    {
        private readonly ChromeDriver _driver;
        private readonly RelationalContext _context;
        private string? _uniqueUsername;
        private string? _password;

        public PlayerEndToEndTests()
        {
            _driver = new ChromeDriver();
            _driver.Navigate().GoToUrl("http://localhost:5173/");
            _driver.Manage().Window.Size = new System.Drawing.Size(949, 743);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(3000);

            _context = PersistanceConfiguration.GetRelationalContext(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIROMENT"));
        }

        [Fact]
        public void Sign_up()
        {
            // Generate a unique username
            _uniqueUsername = "testuser_" + DateTime.Now.Ticks;
            _password = "Testpassword1*";

            IWebElement linkElement = _driver.FindElement(By.XPath("//a[text()='here']"));
            linkElement.Click();

            // Fill out the sign up form
            IWebElement usernameInputElement = _driver.FindElement(By.CssSelector("input[placeholder='Username']"));
            usernameInputElement.Clear();
            usernameInputElement.SendKeys(_uniqueUsername);
            IWebElement passwordInputElement = _driver.FindElement(By.CssSelector("input[placeholder='Password']"));
            passwordInputElement.Clear();
            passwordInputElement.SendKeys(_password);
            IWebElement rePasswordInputElement = _driver.FindElement(By.CssSelector("input[placeholder='Repeat Password']"));
            rePasswordInputElement.Click();
            rePasswordInputElement.SendKeys(_password);

            IWebElement signUpButtonElement = _driver.FindElement(By.XPath("//button[contains(text(),'Sign up')]"));
            signUpButtonElement.Click();

            // Assert that the success message is displayed
            IWebElement successMessage = _driver.FindElement(By.XPath("//div[@role='status']//div[contains(@class, 'chakra-alert') and contains(text(),'User created successfully')]"));
            Assert.True(successMessage.Displayed, "User creation success message is not displayed");
        }

        [Fact]
        public void Sign_in()
        {
            CreateAndSigninTestUser();

            IWebElement element = _driver.FindElement(By.XPath("//p[text()='Fight!']"));
            string text = element.Text;

            Assert.Equal("Fight!", text);
        }

        [Fact]
        public void Sign_out()
        {
            CreateAndSigninTestUser();

            // Sign out
            IWebElement signOutButtonElement = _driver.FindElement(By.XPath("//button[contains(text(),'Logout')]"));
            signOutButtonElement.Click();
            IWebElement element = _driver.FindElement(By.XPath("//p[text()='Log in']"));
            string text = element.Text;
            Assert.Equal("Log in", text);
        }

        [Fact]
        public void Create_update_delete_deck()
        {
            CreateAndSigninTestUser();

            // Create deck
            IWebElement decksButtonElement = _driver.FindElement(By.XPath("//p[text()='Decks']"));
            decksButtonElement.Click();
            IWebElement newButtonElement = _driver.FindElement(By.XPath("//button[contains(text(),'New')]"));
            newButtonElement.Click();
            IWebElement playingCard = _driver.FindElement(By.XPath("(//div[@tabindex='0'])[1]"));
            int numberOfCardsToAdd = 3;
            for (int i = 0; i < numberOfCardsToAdd; i++)
            {
                playingCard.Click();
            }
            IWebElement deckCard = _driver.FindElement(By.XPath("(//div[@class='css-1ct4vp3']//div[contains(@class,'chakra-stack')])[1]"));
            deckCard.Click();
            IWebElement numberElement = _driver.FindElement(By.XPath("//div[contains(@class, 'chakra-stack')]//p[contains(text(), '(')]"));
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
            playingCard = _driver.FindElement(By.XPath("(//div[@tabindex='0'])[1]"));
            playingCard.Click();
            numberElement = _driver.FindElement(By.XPath("//div[contains(@class, 'chakra-stack')]//p[contains(text(), '(')]"));
            textContent = numberElement.Text;
            value = int.Parse(textContent.Trim('(', ')'));
            Assert.Equal(3, value);
            saveDeckBtn = _driver.FindElement(By.XPath("//button[text()='Save deck']"));
            saveDeckBtn.Click();

            // Delete deck
            IWebElement deleteButtonElement = _driver.FindElement(By.XPath("//button[contains(text(),'Delete')]"));
            deleteButtonElement.Click();

            // Handle modal
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            IWebElement modal = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".chakra-modal__content")));
            IWebElement modalDeleteButton = modal.FindElement(By.XPath(".//button[contains(text(),'Delete')]"));
            modalDeleteButton.Click();

            // Scroll down and wait until the "testdeck" element is no longer visible
            ((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
            bool isDeleted = wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//div[contains(text(), 'testdeck')]")));
            Assert.True(isDeleted, "The deck 'testdeck' is still visible on the page.");
        }

        [Fact]
        public void Create_comment()
        {
            CreateAndSigninTestUser();

            // Create comment
            IWebElement publicDecksButtonElement = _driver.FindElement(By.XPath("//p[text()='Public Decks']"));
            publicDecksButtonElement.Click();
            IWebElement firstViewCommentsButton = _driver.FindElement(By.XPath("(//button[contains(text(), 'View Comments')])[1]"));
            firstViewCommentsButton.Click();

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
            firstViewCommentsButton = _driver.FindElement(By.XPath("(//button[contains(text(), 'View Comments')])[1]"));
            firstViewCommentsButton.Click();

            // Check if comment was created
            IWebElement commentElement = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//p[text()='This is a test comment']")));
            Assert.True(commentElement.Displayed, "The comment is not visible on the deck.");
        }

        [Fact]
        public void Fight_win()
        {
            CreateAndSigninTestUser();

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
        }

        [Fact]
        public void Fight_lose()
        {
            CreateAndSigninTestUser();

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
        }

        private void CreateAndSigninTestUser()
        {
            // Generate a unique username
            _uniqueUsername = "testuser_" + DateTime.Now.Ticks;
            _password = "Testpassword1*";

            IWebElement linkElement = _driver.FindElement(By.XPath("//a[text()='here']"));
            linkElement.Click();

            // Fill out the sign up form
            IWebElement usernameInputElement = _driver.FindElement(By.CssSelector("input[placeholder='Username']"));
            usernameInputElement.Clear();
            usernameInputElement.SendKeys(_uniqueUsername);
            IWebElement passwordInputElement = _driver.FindElement(By.CssSelector("input[placeholder='Password']"));
            passwordInputElement.Clear();
            passwordInputElement.SendKeys(_password);
            IWebElement rePasswordInputElement = _driver.FindElement(By.CssSelector("input[placeholder='Repeat Password']"));
            rePasswordInputElement.Click();
            rePasswordInputElement.SendKeys(_password);

            IWebElement signUpButtonElement = _driver.FindElement(By.XPath("//button[contains(text(),'Sign up')]"));
            signUpButtonElement.Click();

            // Assert that the success message is displayed
            IWebElement successMessage = _driver.FindElement(By.XPath("//div[@role='status']//div[contains(@class, 'chakra-alert') and contains(text(),'User created successfully')]"));
            Assert.True(successMessage.Displayed, "User creation success message is not displayed");

            linkElement = _driver.FindElement(By.XPath("//a[text()='here']"));
            linkElement.Click();

            // Sign in
            usernameInputElement = _driver.FindElement(By.CssSelector("input[placeholder='Username']"));
            usernameInputElement.Clear();
            usernameInputElement.SendKeys(_uniqueUsername);
            passwordInputElement = _driver.FindElement(By.CssSelector("input[placeholder='Password']"));
            passwordInputElement.Clear();
            passwordInputElement.SendKeys(_password);
            IWebElement signInButtonElement = _driver.FindElement(By.XPath("//button[contains(text(),'Sign in')]"));
            signInButtonElement.Click();
        }

        private void DeleteTestUser()
        {
            var userDB = _context.Users.First(u => u.Username == _uniqueUsername);
            if (userDB == null)
            {
                return;
            }
            userDB.Username = $"thisUserIsDeleted_{Guid.NewGuid():N}";
            _context.SaveChanges();
        }

        public void Dispose()
        {
            DeleteTestUser();

            _driver?.Close();
            _driver?.Quit();
        }
    }
}
