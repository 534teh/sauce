using Reqnroll.BoDi;
using log4net.Config;
using Reqnroll;
using Sauce.Config;
using Sauce.Drivers;

namespace Sauce.Support;

[Binding]
public class Hooks(IObjectContainer objectContainer)
{
    private readonly IObjectContainer _objectContainer = objectContainer;
    private static readonly ConfigurationService ConfigService = new();

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
        var driver = WebDriverFactory.GetDriver(ConfigurationService.GetBrowserSettings());

        this._objectContainer.RegisterInstanceAs(driver);
        this._objectContainer.RegisterInstanceAs(ConfigService);
    }

    [AfterScenario]
    public static void AfterScenario()
    {
        WebDriverFactory.QuitCurrentDriver();
    }
}