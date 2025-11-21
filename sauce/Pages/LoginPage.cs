using OpenQA.Selenium;
using sauce.Config;

namespace sauce.Pages;

/// <summary>
/// Page object representing the Login Page.
/// </summary>
/// <param name="driver">The WebDriver instance.</param>
public class LoginPage(IWebDriver driver, IConfigurationService configService)
    : BasePage(driver, configService.GetFullUrlPage(UrlKey))
{
    private const string UrlKey = "LoginPage";

    private IWebElement UsernameField => this.Driver.FindElement(By.Id("user-name"));
    private IWebElement PasswordField => this.Driver.FindElement(By.Id("password"));
    private IWebElement LoginButton => this.Driver.FindElement(By.Id("login-button"));
    private IWebElement ErrorMessageContainer => this.Driver.FindElement(By.CssSelector(".error-message-container h3"));

    /// <summary>
    /// Enters the username into the username field.
    /// </summary>
    public void EnterUsername(string username)
    {
        this.Log.Debug($"Entering username");

        this.UsernameField.SendKeys(username);
    }

    /// <summary>
    /// Enters the password into the password field.
    /// </summary>
    public void EnterPassword(string password)
    {
        this.Log.Debug("Entering password");

        this.PasswordField.SendKeys(password);
    }

    /// <summary>
    /// Clicks the Login button.
    /// </summary>
    public void ClickLogin()
    {
        this.Log.Info("Clicking the Login button.");

        this.LoginButton.Click();
    }

    /// <summary>
    /// Gets the error message text.
    /// </summary>
    public string GetErrorMessage()
    {
        this.Log.Debug("Retrieving error message text.");

        var errorText = this.ErrorMessageContainer.Text;

        return errorText;
    }

    /// <summary>
    /// Clears the Username field.
    /// </summary>
    public void ClearUsername()
    {
        this.Log.Debug("Clearing Username field.");
        this.ClearElement(this.UsernameField);
    }

    /// <summary>
    /// Clears the Password field.
    /// </summary>
    public void ClearPassword()
    {
        this.Log.Debug("Clearing Password field.");
        this.ClearElement(this.PasswordField);
    }
}
