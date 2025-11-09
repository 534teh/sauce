using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace sauce.Pages
{
    public class InventoryPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public InventoryPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        private IWebElement Title => _wait.Until(d => d.FindElement(By.ClassName("title")));

        public string GetTitle()
        {
            return Title.Text;
        }
    }
}
