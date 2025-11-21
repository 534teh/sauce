using OpenQA.Selenium;
using log4net;

namespace sauce.Pages;

/// <summary>
/// Page object representing the Inventory Page.
/// </summary>
/// <param name="driver">The WebDriver instance.</param>
public abstract class BasePage
{
    /// <summary>
    /// The URL of the Inventory page.
    /// </summary>
    public string Url { get; protected set; }

    /// <summary>
    /// The WebDriver instance.
    /// </summary>
    protected readonly IWebDriver Driver;

    /// <summary>
    /// The logger instance.
    /// </summary>
    protected readonly ILog Log;

    protected BasePage(IWebDriver driver, string url)
    {
        this.Driver = driver;

        this.Log = LogManager.GetLogger(this.GetType());

        this.Url = url;
    }

    /// <summary>
    /// Opens the Inventory page.
    /// </summary>
    /// <returns>The InventoryPage instance.</returns>
    public void Open()
    {
        this.Log.Info($"Navigating to URL: {this.Url}");

        this.Driver.Navigate().GoToUrl(this.Url);
    }

    /// <summary>
    /// Clears the given IWebElement input field.
    /// </summary>
    protected void ClearElement(IWebElement element)
    {
        this.Log.Debug($"Clearing element. Value before clear: '{element.GetAttribute("value")}'");
        element.SendKeys(Keys.Control + "a");
        element.SendKeys(Keys.Delete);
    }
}
