using Reqnroll.BoDi;
using log4net;
using log4net.Config;
using Reqnroll;
using OpenQA.Selenium;
using sauce.Config;
using sauce.Drivers;

namespace sauce.Support;

[Binding]
public class Hooks(IObjectContainer objectContainer)
{
    private readonly IObjectContainer _objectContainer = objectContainer;
    private static readonly ILog Log = LogManager.GetLogger(typeof(Hooks));
    private static readonly IConfigurationService ConfigService = new ConfigurationService();

    [BeforeTestRun]
    public static void BeforeTestRun()
    {
        Environment.SetEnvironmentVariable("BROWSER", "Chrome");
        _ = XmlConfigurator.Configure(new FileInfo("log4net.config"));
    }

    [AfterTestRun]
    public static void AfterTestRun()
    {
        WebDriverFactory.CloseAllDrivers();
    }

    [BeforeScenario]
    public void BeforeScenario()
    {
        var driver = WebDriverFactory.GetDriver(ConfigService.GetBrowserSettings());

        this._objectContainer.RegisterInstanceAs(driver);
        this._objectContainer.RegisterInstanceAs(ConfigService);
    }

    [AfterScenario]
    public void AfterScenario(IWebDriver driver)
    {
        try
        {
            driver.Manage().Cookies.DeleteAllCookies();
            driver.Navigate().GoToUrl("about:blank");
        }
        catch (WebDriverException ex)
        {
            // Continue as we can remake the driver next scenario if needed

            Log.Error("Failed to clean up driver state", ex);

            WebDriverFactory.QuitCurrentDriver();
        }
    }
}