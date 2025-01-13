using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using Infrastructure.Persistance;
using Infrastructure.Persistance.Relational;

namespace EndToEndTests
{
    public class AdminEndToEndTests : IDisposable
    {
        private readonly ChromeDriver _driver;
        private readonly RelationalContext _context;
        private string? _uniqueUsername;
        private string? _password;

        public AdminEndToEndTests()
        {
            _driver = new ChromeDriver();
            _driver.Navigate().GoToUrl("http://localhost:5173/");
            _driver.Manage().Window.Size = new System.Drawing.Size(949, 743);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(3000);

            _context = PersistanceConfiguration.GetRelationalContext(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIROMENT"));
        }

        [Fact]
        public void Sign_in()
        {
            CreateAndSigninTestAdmin();

            IWebElement element = _driver.FindElement(By.XPath("//p[text()='Enemies']"));
            string text = element.Text;

            Assert.Equal("Enemies", text);
        }

        [Fact]
        public void Sign_out()
        {
            CreateAndSigninTestAdmin();

            // Sign out
            IWebElement signOutButtonElement = _driver.FindElement(By.XPath("//button[contains(text(),'Logout')]"));
            signOutButtonElement.Click();
            IWebElement element = _driver.FindElement(By.XPath("//p[text()='Log in']"));
            string text = element.Text;
            Assert.Equal("Log in", text);
        }

        [Fact]
        public void Create_update_delete_enemy()
        {
            CreateAndSigninTestAdmin();

            // Create enemy
            IWebElement enemiesButtonElement = _driver.FindElement(By.XPath("//p[text()='Enemies']"));
            enemiesButtonElement.Click();
            IWebElement newButtonElement = _driver.FindElement(By.XPath("//button[contains(text(),'New')]"));
            newButtonElement.Click();

            // Input fields
            IWebElement nameField = _driver.FindElement(By.CssSelector("input[placeholder='Name']"));
            nameField.Clear();
            nameField.SendKeys("testenemy");
            IWebElement healthField = _driver.FindElement(By.CssSelector("input[placeholder='Health']"));
            healthField.Clear();
            healthField.SendKeys("10");
            IWebElement imagePathField = _driver.FindElement(By.CssSelector("input[placeholder='ImagePath']"));
            imagePathField.Clear();
            imagePathField.SendKeys("testimage");
            IWebElement createEnemyBtn = _driver.FindElement(By.XPath("//button[text()='Create']"));
            createEnemyBtn.Click();

            // Edit enemy
            IWebElement enemyContainerEdit = _driver.FindElement(By.XPath("//div[text()='testenemy']/ancestor::div[contains(@class, 'css-trf0pt')]"));
            IWebElement editButton = enemyContainerEdit.FindElement(By.XPath(".//button[text()='Edit']"));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", editButton);
            editButton.Click();
            imagePathField = _driver.FindElement(By.CssSelector("input[placeholder='ImagePath']"));
            imagePathField.SendKeys("2");
            IWebElement updateEnemyBtn = _driver.FindElement(By.XPath("//button[text()='Update']"));
            updateEnemyBtn.Click();

            // Delete enemy
            IWebElement enemyContainerDelete = _driver.FindElement(By.XPath("//div[text()='testenemy']/ancestor::div[contains(@class, 'css-trf0pt')]"));
            IWebElement deleteButton = enemyContainerDelete.FindElement(By.XPath(".//button[text()='Delete']"));
            deleteButton.Click();

            // Handle modal
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            IWebElement modal = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".chakra-modal__content")));
            IWebElement modalDeleteButton = modal.FindElement(By.XPath(".//button[contains(text(),'Delete')]"));
            modalDeleteButton.Click();

            // Scroll down and wait until the "testenemy" element is no longer visible
            ((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
            bool isDeleted = wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//div[contains(text(), 'testenemy')]")));
            Assert.True(isDeleted, "The enemy 'testenemy' is still visible on the page.");
        }

        [Fact]
        public void Create_update_delete_card()
        {
            CreateAndSigninTestAdmin();

            // Create card
            IWebElement cardsButtonElement = _driver.FindElement(By.XPath("//p[text()='Cards']"));
            cardsButtonElement.Click();
            IWebElement newButtonElement = _driver.FindElement(By.XPath("//button[contains(text(),'New')]"));
            newButtonElement.Click();

            // Input fields
            IWebElement nameField = _driver.FindElement(By.CssSelector("input[placeholder='Name']"));
            nameField.Clear();
            nameField.SendKeys("testcard");
            IWebElement descriptionField = _driver.FindElement(By.CssSelector("input[placeholder='Description']"));
            descriptionField.Clear();
            descriptionField.SendKeys("testdescription");
            IWebElement attackField = _driver.FindElement(By.CssSelector("input[placeholder='Attack']"));
            attackField.Clear();
            attackField.SendKeys("10");
            IWebElement costField = _driver.FindElement(By.CssSelector("input[placeholder='Cost']"));
            costField.Clear();
            costField.SendKeys("1");
            IWebElement defenceField = _driver.FindElement(By.CssSelector("input[placeholder='Defence']"));
            defenceField.Clear();
            defenceField.SendKeys("5");
            IWebElement imagePathField = _driver.FindElement(By.CssSelector("input[placeholder='ImagePath']"));
            imagePathField.Clear();
            imagePathField.SendKeys("testimage");
            IWebElement createEnemyBtn = _driver.FindElement(By.XPath("//button[text()='Create']"));
            createEnemyBtn.Click();

            // Edit card
            IWebElement cardContainerEdit = _driver.FindElement(By.XPath("//div[text()='testcard']/ancestor::div[contains(@class, 'css-trf0pt')]"));
            IWebElement editButton = cardContainerEdit.FindElement(By.XPath(".//button[text()='Edit']"));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", editButton);
            editButton.Click();
            imagePathField = _driver.FindElement(By.CssSelector("input[placeholder='ImagePath']"));
            imagePathField.SendKeys("2");
            IWebElement updateButton = _driver.FindElement(By.XPath("//button[text()='Update']"));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", updateButton);
            updateButton.Click();

            // Delete card
            IWebElement cardContainerDelete = _driver.FindElement(By.XPath("//div[text()='testcard']/ancestor::div[contains(@class, 'css-trf0pt')]"));
            IWebElement deleteButton = cardContainerDelete.FindElement(By.XPath(".//button[text()='Delete']"));
            deleteButton.Click();

            // Handle modal
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            IWebElement modal = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".chakra-modal__content")));
            IWebElement modalDeleteButton = modal.FindElement(By.XPath(".//button[contains(text(),'Delete')]"));
            modalDeleteButton.Click();

            // Scroll down and wait until the "testcard" element is no longer visible
            ((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
            bool isDeleted = wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//div[contains(text(), 'testcard')]")));
            Assert.True(isDeleted, "The card 'testcard' is still visible on the page.");
        }

        private void CreateAndSigninTestAdmin()
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

            var userDB = _context.Users.First(u => u.Username == _uniqueUsername);
            if (userDB == null)
            {
                return;
            }
            userDB.Role = "Admin";
            _context.SaveChanges();

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