using sauce.Pages;
using log4net;
using log4net.Config;
using OpenQA.Selenium;
using sauce.Drivers;

[assembly: Parallelize(Workers = 0, Scope = ExecutionScope.MethodLevel)]

namespace sauce.Tests;

[TestClass]
public class SauceTests
{
    protected IWebDriver Driver = null!;
    protected readonly ILog log = LogManager.GetLogger(typeof(SauceTests));

    [AssemblyInitialize]
    public static void AssemblyInit(TestContext context)
    {
        context.WriteLine("Starting Assembly Initialization...");

        _ = XmlConfigurator.Configure(new FileInfo("log4net.config"));

        context.WriteLine("log4net configuration loaded successfully.");
    }

    [TestInitialize]
    public void Setup()
    {
        this.Driver = WebDriverCreator.Create();
        this.log.Debug("New WebDriver instance created.");
    }

    [TestCleanup]
    public void Cleanup()
    {
        this.Driver?.Quit();
        this.Driver?.Dispose();
        this.log.Debug("WebDriver instance quit.");
    }

    [DataTestMethod]
    [DataRow("Username1", "Password1", "Epic sadface: Username is required")]
    [DataRow("", "", "Epic sadface: Username is required")]
    public void UC1_EmptyCredentialsTest(string username, string password, string expectedError)
    {
        this.log.Info($"Running UC1: Empty Credentials Test. Expected Error: '{expectedError}'");

        var loginPage = new LoginPage(this.Driver).Open();

        loginPage.EnterUsername(username);
        loginPage.EnterPassword(password);

        // We are entering the credentials and then clearing them to test the 'clear' methods
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

        var loginPage = new LoginPage(this.Driver).Open();

        loginPage.EnterUsername(username);
        loginPage.EnterPassword(password);
        // We are entering the password and then clearing it to test the 'ClearPassword' method
        loginPage.ClearPassword();
        loginPage.ClickLogin();

        var actualError = loginPage.GetErrorMessage();

        Assert.AreEqual(expectedError, actualError);

        this.log.Info("UC2 finished.");
    }

    [DataTestMethod]
    [DataRow("standard_user", "secret_sauce", "Swag Labs")]
    [DataRow("problem_user", "secret_sauce", "Swag Labs")]
    [DataRow("performance_glitch_user", "secret_sauce", "Swag Labs")]
    [DataRow("error_user", "secret_sauce", "Swag Labs")]
    [DataRow("visual_user", "secret_sauce", "Swag Labs")]
    public void UC3_ValidCredentialsTest(string username, string password, string expectedTitle)
    {
        this.log.Info($"Running UC3: Valid Credentials Test. Username: '{username}'" +
                      $" password: '{password}', expected title: '{expectedTitle}'");

        var loginPage = new LoginPage(this.Driver).Open();

        loginPage.EnterUsername(username);
        loginPage.EnterPassword(password);
        loginPage.ClickLogin();

        var inventoryPage = new InventoryPage(this.Driver);

        var actualTitle = inventoryPage.GetTitle();

        Assert.AreEqual(expectedTitle, actualTitle);

        this.log.Info("UC3 finished.");
    }
}
