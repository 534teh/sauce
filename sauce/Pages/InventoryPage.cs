using OpenQA.Selenium;
using sauce.Config;

namespace sauce.Pages;

/// <summary>
/// Page object representing the Inventory Page.
/// </summary>
/// <param name="driver">The WebDriver instance.</param>
public class InventoryPage(IWebDriver driver, IConfigurationService configService)
    : BasePage(driver, configService.GetFullUrlPage(UrlKey))
{
    /// <summary>
    /// The URL key for the Inventory page.
    /// </summary>
    private const string UrlKey = "InventoryPage";

    private IWebElement titleElement => this.Driver.FindElement(By.XPath("//div[@class='app_logo']"));

    /// <summary>
    /// Gets the title of the Inventory page.
    /// </summary>
    /// <returns>The title text.</returns>
    public string GetTitle()
    {
        return this.titleElement.Text;
    }
}
