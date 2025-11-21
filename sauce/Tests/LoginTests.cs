using sauce.Pages;
using log4net;
using log4net.Config;
using OpenQA.Selenium;
using sauce.Config;
using sauce.Drivers;

namespace sauce.Tests;

[TestClass]
public class SauceTests
{
    protected readonly ILog log = LogManager.GetLogger(typeof(SauceTests));
    protected static readonly IConfigurationService ConfigurationService = new ConfigurationService();
    protected IWebDriver Driver = WebDriverFactory.GetDriver(ConfigurationService.GetBrowserSettings());

    [AssemblyInitialize]
    public static void AssemblyInit(TestContext context)
    {
        Environment.SetEnvironmentVariable("BROWSER", "Chrome");
        _ = XmlConfigurator.Configure(new FileInfo("log4net.config"));

        context.WriteLine("log4net configuration loaded successfully.");
    }

    [AssemblyCleanup]
    public static void AssemblyClean()
    {
        WebDriverFactory.CloseAllDrivers();
    }

    [TestCleanup]
    public void TestClean()
    {
        try
        {
            this.Driver.Manage().Cookies.DeleteAllCookies();
            this.Driver.Navigate().GoToUrl("about:blank");
        }
        catch (WebDriverException ex)
        {
            this.log.Error("Failed to clean up driver state", ex);
        }
    }

    [DataTestMethod]
    [DataRow("Username1", "Password1", "Epic sadface: Username is required")]
    [DataRow("", "", "Epic sadface: Username is required")]
    public void UC1_EmptyCredentialsTest(string username, string password, string expectedError)
    {
        this.log.Info($"Running UC1: Empty Credentials Test. Expected Error: '{expectedError}'");

        var loginPage = new LoginPage(this.Driver, ConfigurationService);
        loginPage.Open();

        loginPage.EnterUsername(username);
        loginPage.EnterPassword(password);

        loginPage.ClearUsername();
        loginPage.ClearPassword();
        loginPage.ClickLogin();

        var actualError = loginPage.GetErrorMessage();

        Assert.AreEqual(expectedError, actualError);

        this.log.Info("UC1 finished.");
    }

    [DataTestMethod]
    [DataRow("Username1", "Password1", "Epic sadface: Password is required")]
    [DataRow("Username1", "", "Epic sadface: Password is required")]
    public void UC2_EmptyPasswordTest(string username, string password, string expectedError)
    {
        this.log.Info($"Running UC2: Empty Password Test. Username: '{username}', password: '{password}'");

        var loginPage = new LoginPage(this.Driver, ConfigurationService);
        loginPage.Open();

        loginPage.EnterUsername(username);
        loginPage.EnterPassword(password);

        loginPage.ClearPassword();
        loginPage.ClickLogin();

        var actualError = loginPage.GetErrorMessage();

        Assert.AreEqual(expectedError, actualError);

        this.log.Info("UC2 finished.");
    }

    [DataTestMethod]
    [DataRow("problem_user", "secret_sauce", "Swag Labs")]
    [DataRow("performance_glitch_user", "secret_sauce", "Swag Labs")]
    [DataRow("standard_user", "secret_sauce", "Swag Labs")]
    [DataRow("error_user", "secret_sauce", "Swag Labs")]
    [DataRow("visual_user", "secret_sauce", "Swag Labs")]
    //[DataRow("locked_out_user", "secret_sause", "Epic sadface: Sorry, this user has been locked out.")]
    public void UC3_ValidCredentialsTest(string username, string password, string expectedTitle)
    {
        this.log.Info($"Running UC3: Valid Credentials Test. Username: '{username}'" +
                      $" password: '{password}', expected title: '{expectedTitle}'");

        var loginPage = new LoginPage(this.Driver, ConfigurationService);
        loginPage.Open();

        loginPage.EnterUsername(username);
        loginPage.EnterPassword(password);
        loginPage.ClickLogin();

        var inventoryPage = new InventoryPage(this.Driver, ConfigurationService);

        var actualTitle = inventoryPage.GetTitle();

        Assert.AreEqual(expectedTitle, actualTitle);

        this.log.Info("UC3 finished.");
    }
}
