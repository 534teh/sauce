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
    protected IWebDriver Driver = WebDriverManager.Driver;
    protected readonly ILog log = LogManager.GetLogger(typeof(SauceTests));

    [AssemblyInitialize]
    public static void AssemblyInit(TestContext context)
    {
        context.WriteLine("Starting Assembly Initialization...");

        _ = XmlConfigurator.Configure(new FileInfo("log4net.config"));

        context.WriteLine("log4net configuration loaded successfully.");
    }

    [TestCleanup]
    public void Cleanup()
    {
        WebDriverManager.QuitDriver();
    }

    [DataTestMethod]
    [DataRow("Username1", "Password1", "Epic sadface: Username is required")]
    public void UC1_EmptyCredentialsTest(string username, string password, string expectedError)
    {
        this.log.Info($"Running UC1: Empty Credentials Test. Expected Error: '{expectedError}'");

        var loginPage = new LoginPage(this.Driver).Open();

        loginPage.EnterUsername(username);
        loginPage.EnterPassword(password);

        loginPage.ClearUsername();
        loginPage.ClearPassword();

        loginPage.ClickLogin();

        var actualError = loginPage.GetErrorMessage();
        try
        {
            Assert.AreEqual(expectedError, actualError);
            this.log.Info("Assertion passed: Correct error message displayed.");
        }
        catch (AssertFailedException ex)
        {
            this.log.Error($"UC1 failed. Expected: '{expectedError}', Actual: '{actualError}'", ex);
            throw;
        }

        this.log.Info("UC1 finished.");
    }

    [DataTestMethod]
    [DataRow("Username1", "Password1", "Epic sadface: Password is required")]
    public void UC2_EmptyPasswordTest(string username, string password, string expectedError)
    {
        this.log.Info($"Running UC2 with username: '{username}', password: '{password}'");

        var loginPage = new LoginPage(this.Driver).Open();

        loginPage.EnterUsername(username);
        loginPage.EnterPassword(password);

        loginPage.ClearPassword();

        loginPage.ClickLogin();

        var actualError = loginPage.GetErrorMessage();
        try
        {
            Assert.AreEqual(expectedError, actualError);
            this.log.Info($"Assertion passed: Error message is: '{actualError}'.");
        }
        catch (AssertFailedException ex)
        {
            this.log.Error($"UC2 failed. Expected: '{expectedError}', Actual: '{actualError}'", ex);
            throw;
        }

        this.log.Info("UC2 finished.");
    }

    [DataTestMethod]
    [DataRow("standard_user", "secret_sauce", "Swag Labs")]
    public void UC3_ValidCredentialsTest(string username, string password, string expectedTitle)
    {
        this.log.Info($"Running UC3: Valid Login Test. Expected Title: '{expectedTitle}'");

        var loginPage = new LoginPage(this.Driver).Open();

        loginPage.EnterUsername(username);
        loginPage.EnterPassword(password);
        loginPage.ClickLogin();

        var inventoryPage = new InventoryPage(this.Driver); // No need to call Open(), already redirected after login

        var actualTitle = inventoryPage.GetTitle();
        try
        {
            Assert.AreEqual(expectedTitle, actualTitle);

            this.log.Info($"Assertion passed: Inventory page title is '{actualTitle}'.");
        }
        catch (AssertFailedException ex)
        {
            this.log.Error($"UC3 failed. Expected title: '{expectedTitle}', Actual title: '{actualTitle}'", ex);
            throw;
        }

        this.log.Info("UC3 finished.");
    }
}
