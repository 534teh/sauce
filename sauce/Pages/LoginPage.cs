using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using log4net;

namespace sauce.Pages;

/// <summary>
/// Page object representing the Login Page.
/// </summary>
/// <param name="driver">The WebDriver instance.</param>
public class LoginPage(IWebDriver driver)
{
    /// <summary>
    /// The URL of the Login page.
    /// </summary>
    public const string Url = "https://www.saucedemo.com/";
    private readonly IWebDriver _driver = driver;

    private static readonly ILog _log = LogManager.GetLogger(typeof(LoginPage));

    private IWebElement UsernameField => this._driver.FindElement(By.Id("user-name"));
    private IWebElement PasswordField => this._driver.FindElement(By.Id("password"));
    private IWebElement LoginButton => this._driver.FindElement(By.Id("login-button"));
    private IWebElement ErrorMessageContainer => this._driver.FindElement(By.CssSelector(".error-message-container h3"));

    /// <summary>
    /// Opens the Login page.
    /// </summary>
    /// <returns>The LoginPage instance.</returns>
    public LoginPage Open()
    {
        _log.Info($"Navigating to URL: {Url}");

        this._driver.Navigate().GoToUrl(Url);
        this.WaitForPageToLoad();

        return this;
    }

    /// <summary>
    /// Enters the username into the username field.
    /// </summary>
    public void EnterUsername(string username)
    {
        _log.Debug($"Entering username");

        this.UsernameField.SendKeys(username);
    }

    /// <summary>
    /// Enters the password into the password field.
    /// </summary>
    public void EnterPassword(string password)
    {
        _log.Debug("Entering password");

        this.PasswordField.SendKeys(password);
    }

    /// <summary>
    /// Clicks the Login button.
    /// </summary>
    public void ClickLogin()
    {
        _log.Info("Clicking the Login button.");

        this.LoginButton.Click();
    }

    /// <summary>
    /// Gets the error message text.
    /// </summary>
    public string GetErrorMessage()
    {
        _log.Debug("Retrieving error message text.");

        var errorText = this.ErrorMessageContainer.Text;

        _log.Debug($"Retrieved error message: '{errorText}'");

        return errorText;
    }

    /// <summary>
    /// Clears the Username field.
    /// </summary>
    public void ClearUsername()
    {
        _log.Debug($"Clearing Username field. Value before clear: '{this.UsernameField.GetAttribute("value")}'");
        ClearElement(this.UsernameField);
    }

    /// <summary>
    /// Clears the Password field.
    /// </summary>
    public void ClearPassword()
    {
        _log.Debug($"Clearing Password field. Value before");
        ClearElement(this.PasswordField);
    }

    /// <summary>
    /// Waits for the Login page to fully load.
    /// </summary>
    private void WaitForPageToLoad()
    {
        _log.Debug("Waiting for the Login page to fully load...");

        var wait = new WebDriverWait(this._driver, TimeSpan.FromSeconds(2));
        _ = wait.Until(d => d.FindElement(By.Id("login-button")));

        _log.Debug("Login page elements are visible.");
    }

    /// <summary>
    /// Clears the given IWebElement input field.
    /// </summary>
    private static void ClearElement(IWebElement element)
    {
        _log.Debug($"Clearing element. Value before clear: '{element.GetAttribute("value")}'");
        element.SendKeys(Keys.Control + "a");
        element.SendKeys(Keys.Delete);
    }
}
