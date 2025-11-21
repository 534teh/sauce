using OpenQA.Selenium;
using sauce.Config;
using OpenQA.Selenium.Support.UI;

namespace sauce.Pages;

/// <summary>
/// Page object representing the Inventory Page.
/// </summary>
/// <param name="driver">The WebDriver instance.</param>
public class InventoryPage(IWebDriver driver, IConfigurationService configService)
    : BasePage(driver, configService.GetFullUrlPage(UrlKey))
{
    private const string UrlKey = "InventoryPage";

    /// <summary>
    /// Gets the title of the Inventory page.
    /// </summary>
    /// <returns>The title text.</returns>
    public string GetTitle()
    {
        this.Log.Debug("Waiting for Inventory page title to appear...");

        // 2. Create an Explicit Wait
        var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(10));

        // 3. Wait until the element exists in the DOM and return it
        var titleElement = wait.Until(d => d.FindElement(By.XPath("//div[@class='app_logo']")));

        this.Log.Debug($"Retrieved title: '{titleElement.Text}'");
        return titleElement.Text;
    }
}
