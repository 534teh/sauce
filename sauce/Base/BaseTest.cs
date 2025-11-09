using log4net;
using log4net.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using sauce.Drivers;
using sauce.Pages;

[assembly: Parallelize(Workers = 0, Scope = ExecutionScope.MethodLevel)]
namespace sauce.Base
{
    [TestClass]
    public class BaseTest
    {
        protected IWebDriver? Driver;
        protected LoginPage? LoginPage;
        protected InventoryPage? InventoryPage;
        protected static readonly ILog log = LogManager.GetLogger(typeof(BaseTest));

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            XmlConfigurator.Configure(new FileInfo("log4net.config"));
        }

        [TestInitialize]
        public void TestSetup()
        {
            log.Info("Starting test setup.");
            Driver = WebDriverManager.Driver;
            Driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            LoginPage = new LoginPage(Driver);
            InventoryPage = new InventoryPage(Driver);
            log.Info("Test setup complete.");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            log.Info("Starting test cleanup.");
            WebDriverManager.QuitDriver();
            log.Info("Test cleanup complete.");
        }
    }
}
