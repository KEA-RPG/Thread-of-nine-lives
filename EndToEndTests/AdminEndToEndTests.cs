using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace EndToEndTests
{
    public class AdminEndToEndTests
    {
        private readonly IWebDriver _driver;

        public AdminEndToEndTests()
        {
            _driver = new ChromeDriver();

            // !!Change the URL to match the URL of our deployed application!!
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
        public void Sign_out()
        {
            IWebElement usernameInputElement = _driver.FindElement(By.CssSelector("input[placeholder='Username']"));
            usernameInputElement.Clear();
            usernameInputElement.SendKeys("testuser");
            IWebElement passwordInputElement = _driver.FindElement(By.CssSelector("input[placeholder='Password']"));
            passwordInputElement.Clear();
            passwordInputElement.SendKeys("testpassword");
            IWebElement signInButtonElement = _driver.FindElement(By.XPath("//button[contains(text(),'Sign in')]"));
            signInButtonElement.Click();

            IWebElement signOutButtonElement = _driver.FindElement(By.XPath("//button[contains(text(),'Logout')]"));
            signOutButtonElement.Click();

            IWebElement element = _driver.FindElement(By.XPath("//p[text()='Thread of Nine Lives']"));
            string text = element.Text;

            Assert.Equal("Thread of Nine Lives", text);
            _driver.Close();
        }

        [Fact]
        public void Sign_in()
        {
            IWebElement usernameInputElement = _driver.FindElement(By.CssSelector("input[placeholder='Username']"));
            usernameInputElement.Clear();
            usernameInputElement.SendKeys("admtestuser");
            IWebElement passwordInputElement = _driver.FindElement(By.CssSelector("input[placeholder='Password']"));
            passwordInputElement.Clear();
            passwordInputElement.SendKeys("testpassword");
            IWebElement signInButtonElement = _driver.FindElement(By.XPath("//button[contains(text(),'Sign in')]"));
            signInButtonElement.Click();

            IWebElement element = _driver.FindElement(By.XPath("//p[text()='Main Menu']"));
            string text = element.Text;

            Assert.Equal("Main Menu", text);
            _driver.Close();
        }

        [Fact]
        public void Create_update_delete_enemy()
        {
            // Sign in
            IWebElement usernameInputElement = _driver.FindElement(By.CssSelector("input[placeholder='Username']"));
            usernameInputElement.Clear();
            usernameInputElement.SendKeys("admtestuser");
            IWebElement passwordInputElement = _driver.FindElement(By.CssSelector("input[placeholder='Password']"));
            passwordInputElement.Clear();
            passwordInputElement.SendKeys("testpassword");
            IWebElement signInButtonElement = _driver.FindElement(By.XPath("//button[contains(text(),'Sign in')]"));
            signInButtonElement.Click();

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
            IWebElement nameField2 = _driver.FindElement(By.CssSelector("input[placeholder='ImagePath']"));
            nameField2.SendKeys("2");
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

            // Wait until the "testenemy" element is no longer visible
            bool isDeleted = wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//div[contains(text(), 'testenemy')]")));
            Assert.True(isDeleted, "The enemy 'testenemy' is still visible on the page.");

            _driver.Close();
        }

        [Fact]
        public void Create_update_delete_card()
        {
            // Sign in
            IWebElement usernameInputElement = _driver.FindElement(By.CssSelector("input[placeholder='Username']"));
            usernameInputElement.Clear();
            usernameInputElement.SendKeys("admtestuser");
            IWebElement passwordInputElement = _driver.FindElement(By.CssSelector("input[placeholder='Password']"));
            passwordInputElement.Clear();
            passwordInputElement.SendKeys("testpassword");
            IWebElement signInButtonElement = _driver.FindElement(By.XPath("//button[contains(text(),'Sign in')]"));
            signInButtonElement.Click();

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
            IWebElement nameField2 = _driver.FindElement(By.CssSelector("input[placeholder='ImagePath']"));
            nameField2.SendKeys("2");
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

            _driver.Close();
        }
    }
}