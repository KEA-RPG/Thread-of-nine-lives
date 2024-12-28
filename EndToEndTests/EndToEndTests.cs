using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

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
            var title = _driver.Title;

            Assert.Equal("Thread of nine lives", title);
        }
    }
}