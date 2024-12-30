using System;
using System.Reflection.Metadata;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using static System.Net.Mime.MediaTypeNames;

namespace EndToEndTests
{
    public class EndToEndTests
    {
        private readonly IWebDriver _driver;

        public EndToEndTests()
        {
            _driver = new ChromeDriver();

            // !!Change the URL to match the URL of our deployed application!!
            _driver.Navigate().GoToUrl("http://localhost:5173/");

            _driver.Manage().Window.Size = new System.Drawing.Size(949, 743);

            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(3000);
        }

        [Fact]
        public void Page_title()
        {
            IWebElement element = _driver.FindElement(By.XPath("//p[text()='Thread of Nine Lives']"));

            string text = element.Text;

            Assert.Equal("Thread of Nine Lives", text);
            _driver.Close();
        }

        [Fact]
        public void Sign_up_player()
        {
            IWebElement linkElement = _driver.FindElement(By.CssSelector("a.chakra-link.css-spn4bz"));
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
        public void Sign_in_player()
        {
            IWebElement usernameInputElement = _driver.FindElement(By.CssSelector("input[placeholder='Username']"));
            usernameInputElement.Clear();
            usernameInputElement.SendKeys("testuser");
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
        public void Sign_out_player()
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
        public void Create_edit_delete_deck()
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
            IWebElement playingCard = _driver.FindElement(By.XPath("//h4[text()='cat']/ancestor::div[@tabindex='0']"));
            playingCard.Click();
            playingCard.Click();
            playingCard.Click();
            IWebElement deckCard = _driver.FindElement(By.XPath("//p[text()='cat']"));
            deckCard.Click();
            IWebElement numberElement = _driver.FindElement(By.XPath("//div[contains(@class, 'chakra-stack') and descendant::p[text()='cat']]//p[contains(text(), '(')]"));
            string textContent = numberElement.Text;
            int value = int.Parse(textContent.Trim('(', ')'));
            Assert.Equal(2, value);
            IWebElement clearDeckButton = _driver.FindElement(By.XPath("//button[text()='Clear Deck']"));
            clearDeckButton.Click();
            playingCard.Click();
            playingCard.Click();
            IWebElement deckNameInput = _driver.FindElement(By.CssSelector("input[placeholder='Deck name']"));
            deckNameInput.SendKeys("testdeck");
            IWebElement checkboxControl = _driver.FindElement(By.CssSelector(".chakra-checkbox__control"));
            checkboxControl.Click();
            IWebElement saveDeckBtn = _driver.FindElement(By.XPath("//button[text()='Save deck']"));
            saveDeckBtn.Click();

            // Edit deck
            IWebElement editButtonElement = _driver.FindElement(By.XPath("//button[contains(text(),'Edit')]"));
            editButtonElement.Click();
            IWebElement playingCard2 = _driver.FindElement(By.XPath("//h4[text()='cat']/ancestor::div[@tabindex='0']"));
            playingCard2.Click();
            IWebElement numberElement2 = _driver.FindElement(By.XPath("//div[contains(@class, 'chakra-stack') and descendant::p[text()='cat']]//p[contains(text(), '(')]"));
            string textContent2 = numberElement2.Text;
            int value2 = int.Parse(textContent2.Trim('(', ')'));
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
    }
}