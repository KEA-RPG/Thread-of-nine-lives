using System;
using System.Reflection.Metadata;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using static System.Net.Mime.MediaTypeNames;

namespace EndToEndTests
{
    public class EndToEndTests
    {
        private readonly IWebDriver _driver;

        public EndToEndTests()
        {
            _driver = new ChromeDriver();

            _driver.Navigate().GoToUrl("https://thread-of-nine-lives.vercel.app/");

            _driver.Manage().Window.Size = new System.Drawing.Size(949, 743);

            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(3000);
        }

        [Fact]
        public void Pass_page_title()
        {
            IWebElement element = _driver.FindElement(By.CssSelector("p.chakra-text.css-0"));

            string text = element.Text;

            Assert.Equal("Thread of Nine Lives", text);
            _driver.Close();
        }

        [Fact]
        public void Pass_sign_up()
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
        public void Pass_sign_in()
        {
            IWebElement usernameInputElement = _driver.FindElement(By.CssSelector("input[placeholder='Username']"));
            usernameInputElement.Clear();
            usernameInputElement.SendKeys("testuser");

            IWebElement passwordInputElement = _driver.FindElement(By.CssSelector("input[placeholder='Password']"));
            passwordInputElement.Clear();
            passwordInputElement.SendKeys("testpassword");

            IWebElement signInButtonElement = _driver.FindElement(By.XPath("//button[contains(text(),'Sign in')]"));
            signInButtonElement.Click();

            _driver.Close();
        }
    }
}