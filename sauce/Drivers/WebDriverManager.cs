using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;
using System;
using System.Threading;

namespace sauce.Drivers
{
    public class WebDriverManager
    {
        private static readonly ThreadLocal<IWebDriver?> _driver = new ThreadLocal<IWebDriver?>(() =>
        {
            var browser = Environment.GetEnvironmentVariable("BROWSER")?.ToLower() ?? "chrome";
            switch (browser)
            {
                case "firefox":
                    var firefoxOptions = new FirefoxOptions();
                    firefoxOptions.AddArgument("--headless");
                    return new FirefoxDriver(firefoxOptions);
                case "edge":
                    var edgeOptions = new EdgeOptions();
                    edgeOptions.AddArgument("--headless");
                    return new EdgeDriver(edgeOptions);
                case "chrome":
                default:
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArgument("--headless");
                    chromeOptions.AddArgument("--no-sandbox"); // Required for running as root in Docker
                    chromeOptions.AddArgument("--disable-gpu"); // Often recommended for headless
                    chromeOptions.AddArgument("--disable-dev-shm-usage"); // Prevent out-of-memory errors
                    return new ChromeDriver(chromeOptions);
            }
        });

        private WebDriverManager() { }

        public static IWebDriver Driver => _driver.Value!;

        public static void QuitDriver()
        {
            if (_driver.IsValueCreated && _driver.Value != null)
            {
                _driver.Value.Quit();
                _driver.Value = null;
            }
        }
    }
}
