using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using log4net;

namespace sauce.Pages;

public class LoginPage(IWebDriver driver)
{
    public const string Url = "https://www.saucedemo.com/";
    private readonly IWebDriver _driver = driver;

    private static readonly ILog _log = LogManager.GetLogger(typeof(LoginPage));

    private IWebElement UsernameField => this._driver.FindElement(By.Id("user-name"));
    private IWebElement PasswordField => this._driver.FindElement(By.Id("password"));
    private IWebElement LoginButton => this._driver.FindElement(By.Id("login-button"));
    private IWebElement ErrorMessageContainer => this._driver.FindElement(By.CssSelector(".error-message-container h3"));

    private void WaitForPageToLoad()
    {
        _log.Debug("Waiting for the Login page to fully load...");

        var wait = new WebDriverWait(this._driver, TimeSpan.FromSeconds(2));
        _ = wait.Until(d => d.FindElement(By.Id("login-button")));

        _log.Debug("Login page elements are visible.");
    }

    public LoginPage Open()
    {
        _log.Info($"Navigating to URL: {Url}");

        this._driver.Navigate().GoToUrl(Url);
        this.WaitForPageToLoad();

        return this;
    }

    public void EnterUsername(string username)
    {
        _log.Debug($"Entering username");

        this.UsernameField.SendKeys(username);
    }

    public void EnterPassword(string password)
    {
        _log.Debug("Entering password");

        this.PasswordField.SendKeys(password);
    }

    public void ClickLogin()
    {
        _log.Info("Clicking the Login button.");

        this.LoginButton.Click();
    }

    public string GetErrorMessage()
    {
        _log.Debug("Retrieving error message text.");

        var errorText = this.ErrorMessageContainer.Text;

        _log.Debug($"Retrieved error message: '{errorText}'");

        return errorText;
    }

    public void ClearUsername()
    {
        _log.Debug($"Clearing Username field. Value before clear: '{this.UsernameField.GetAttribute("value")}'");
        ClearElement(this.UsernameField);
    }

    public void ClearPassword()
    {
        _log.Debug($"Clearing Password field. Value before");
        ClearElement(this.PasswordField);
    }

    private static void ClearElement(IWebElement element)
    {
        _log.Debug($"Clearing element. Value before clear: '{element.GetAttribute("value")}'");
        element.SendKeys(Keys.Control + "a");
        element.SendKeys(Keys.Delete);
    }
}
