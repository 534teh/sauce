using OpenQA.Selenium;
using Sauce.Config;

namespace Sauce.Pages;

/// <summary>
/// Page object representing the Inventory Page.
/// </summary>
/// <param name="driver">The WebDriver instance.</param>
public class InventoryPage(IWebDriver driver)
    : BasePage(driver, ConfigurationService.GetFullUrlPage(UrlKey))
{
    /// <summary>
    /// The URL key for the Inventory page.
    /// </summary>
    private const string UrlKey = "InventoryPage";

    private IWebElement TitleElement => this.Driver.FindElement(By.XPath("//div[@class='app_logo']"));

    /// <summary>
    /// Gets the title of the Inventory page.
    /// </summary>
    /// <returns>The title text.</returns>
    public string GetTitle()
    {
        return this.TitleElement.Text;
    }
}
