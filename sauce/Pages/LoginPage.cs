using OpenQA.Selenium;

namespace sauce.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;

        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
        }

        private IWebElement UsernameField => _driver.FindElement(By.Id("user-name"));
        private IWebElement PasswordField => _driver.FindElement(By.Id("password"));
        private IWebElement LoginButton => _driver.FindElement(By.Id("login-button"));
        private IWebElement ErrorMessageContainer => _driver.FindElement(By.CssSelector(".error-message-container h3"));

        public void EnterUsername(string username)
        {
            UsernameField.SendKeys(username);
        }

        public void EnterPassword(string password)
        {
            PasswordField.SendKeys(password);
        }

        public void ClickLogin()
        {
            LoginButton.Click();
        }

        public string GetErrorMessage()
        {
            return ErrorMessageContainer.Text;
        }

        public void ClearUsername()
        {
            UsernameField.Clear();
        }

        public void ClearPassword()
        {
            PasswordField.Clear();
        }
    }
}
