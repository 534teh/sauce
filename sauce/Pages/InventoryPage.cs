using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using log4net;

namespace sauce.Pages;

/// <summary>
/// Page object representing the Inventory Page.
/// </summary>
/// <param name="driver">The WebDriver instance.</param>
public class InventoryPage(IWebDriver driver)
{
    /// <summary>
    /// The URL of the Inventory page.
    /// </summary>
    public const string Url = "https://www.saucedemo.com/inventory.html";
    private readonly IWebDriver _driver = driver;
    private static readonly ILog _log = LogManager.GetLogger(typeof(LoginPage));

    /// <summary>
    /// Waits for the Inventory page to fully load.
    /// </summary>
    private void WaitForPageToLoad()
    {
        _log.Debug("Waiting for the Login page to fully load...");

        var titleWait = new WebDriverWait(this._driver, TimeSpan.FromSeconds(2));

        _ = titleWait.Until(d => d.FindElement(By.XPath("//div[@class='app_logo']")));
        _log.Debug("Login page elements are visible.");
    }

    /// <summary>
    /// Opens the Inventory page.
    /// </summary>
    /// <returns>The InventoryPage instance.</returns>
    public InventoryPage Open()
    {
        _log.Info($"Navigating to URL: {Url}");

        this._driver.Navigate().GoToUrl(Url);
        this.WaitForPageToLoad();

        return this;
    }

    /// <summary>
    /// Gets the title of the Inventory page.
    /// </summary>
    /// <returns>The title text.</returns>
    public string GetTitle()
    {
        _log.Debug("Retrieving Inventory page title.");

        var title = this._driver.FindElement(By.XPath("//div[@class='app_logo']"));

        _log.Debug($"Retrieved title: '{title.Text}'");

        return title.Text;
    }
}

