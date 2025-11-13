using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using log4net;

namespace sauce.Pages;

public class InventoryPage(IWebDriver driver)
{
    public const string Url = "https://www.saucedemo.com/inventory.html";
    private readonly IWebDriver _driver = driver;
    private static readonly ILog _log = LogManager.GetLogger(typeof(LoginPage));

    private void WaitForPageToLoad()
    {
        _log.Debug("Waiting for the Login page to fully load...");

        var titleWait = new WebDriverWait(this._driver, TimeSpan.FromSeconds(2));

        _ = titleWait.Until(d => d.FindElement(By.XPath("//div[@class='app_logo']")));
        _log.Debug("Login page elements are visible.");
    }

    public InventoryPage Open()
    {
        _log.Info($"Navigating to URL: {Url}");

        this._driver.Navigate().GoToUrl(Url);
        this.WaitForPageToLoad();

        return this;
    }

    public string GetTitle()
    {
        _log.Debug("Retrieving Inventory page title.");

        var title = this._driver.FindElement(By.XPath("//div[@class='app_logo']"));

        _log.Debug($"Retrieved title: '{title.Text}'");

        return title.Text;
    }
}

