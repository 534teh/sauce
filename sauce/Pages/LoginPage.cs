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
    /// <summary>
    /// The URL key for the Login page.
    /// </summary>
    private const string UrlKey = "LoginPage";

    private IWebElement UsernameField => this.Driver.FindElement(By.Id("user-name"));
    private IWebElement PasswordField => this.Driver.FindElement(By.Id("password"));
    private IWebElement LoginButton => this.Driver.FindElement(By.Id("login-button"));
    private IWebElement ErrorMessageContainer => this.Driver.FindElement(By.CssSelector(".error-message-container h3"));

    /// <summary>
    /// Enters both username and password into their respective fields.
    /// </summary>
    public void EnterCredentials(string username, string password)
    {
        this.EnterUsername(username);
        this.EnterPassword(password);
    }

    /// <summary>
    /// Enters the username into the username field.
    /// </summary>
    public void EnterUsername(string username)
    {
        this.Log.Info($"Attempting login with user: '{username}'");

        this.UsernameField.SendKeys(username);
    }

    /// <summary>
    /// Enters the password into the password field.
    /// </summary>
    public void EnterPassword(string password)
    {
        this.PasswordField.SendKeys(password);
    }

    /// <summary>
    /// Clicks the Login button.
    /// </summary>
    public void ClickLogin()
    {
        this.LoginButton.Click();
    }

    /// <summary>
    /// Gets the error message text.
    /// </summary>
    public string GetErrorMessage()
    {
        return this.ErrorMessageContainer.Text;
    }

    /// <summary>
    /// Clears the Username field.
    /// </summary>
    public void ClearUsername()
    {
        ClearElement(this.UsernameField);
    }

    /// <summary>
    /// Clears the Password field.
    /// </summary>
    public void ClearPassword()
    {
        ClearElement(this.PasswordField);
    }
}
