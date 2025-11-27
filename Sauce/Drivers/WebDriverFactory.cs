using OpenQA.Selenium;
using Sauce.Config;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;

using System.Collections.Concurrent;

namespace Sauce.Drivers;

// <summary>
/// Factory class to create WebDriver instances based on environment configuration.
/// </summary>
public static class WebDriverFactory
{
    /// <summary>
    /// Environment variable name for selecting the browser.
    /// </summary>
    private const string BrowserEnvVar = "BROWSER";

    private static readonly ThreadLocal<IWebDriver> _threadLocalDriver = new();

    private static readonly ConcurrentBag<IWebDriver> _allDrivers = [];

    /// <summary>
    /// Gets the WebDriver instance for the current thread.
    /// </summary>
    /// <returns>IWebDriver instance.</returns>
    public static IWebDriver GetDriver(BrowserSettings browserSettings)
    {
        if (_threadLocalDriver.Value == null)
        {
            var newDriver = CreateDriverInstance(browserSettings);

            _threadLocalDriver.Value = newDriver;

            _allDrivers.Add(newDriver);
        }

        return _threadLocalDriver.Value;
    }

    /// <summary>
    /// Quits and disposes the WebDriver instance for the current thread.
    /// </summary>
    public static void QuitCurrentDriver()
    {
        var driver = _threadLocalDriver.Value;
        if (driver != null)
        {
            driver.Quit();
            driver.Dispose();
            _threadLocalDriver.Value = null!;
        }
    }

    /// <summary>
    /// Closes and disposes all WebDriver instances created by the factory.
    /// </summary>
    public static void CloseAllDrivers()
    {
        foreach (var driver in _allDrivers)
        {
            driver.Quit();
            driver.Dispose();
        }
    }

    private static IWebDriver CreateDriverInstance(BrowserSettings browserSettings)
    {
        var envBrowser = Environment.GetEnvironmentVariable(BrowserEnvVar);
        var browserKey = envBrowser ?? browserSettings.FallbackBrowser;

        if (!browserSettings.Browsers.TryGetValue(browserKey, out var settings))
        {
            throw new ArgumentException($"Configuration for browser '{browserKey}' not found.");
        }

        return settings.BrowserName switch
        {
            "Chrome" => CreateChromeDriver(settings),
            "Firefox" => CreateFirefoxDriver(settings),
            "Edge" => CreateEdgeDriver(settings),
            _ => throw new NotSupportedException($"Browser '{settings.BrowserName}' is not supported.")
        };
    }

    private static ChromeDriver CreateChromeDriver(BrowserSpecificSettings settings)
    {
        var options = new ChromeOptions();

        options.AddArguments(settings.Arguments);

        var driver = new ChromeDriver(options);

        return driver;
    }

    private static FirefoxDriver CreateFirefoxDriver(BrowserSpecificSettings settings)
    {
        var options = new FirefoxOptions();

        options.AddArguments(settings.Arguments);

        var driver = new FirefoxDriver(options);

        return driver;
    }

    private static EdgeDriver CreateEdgeDriver(BrowserSpecificSettings settings)
    {
        var options = new EdgeOptions();

        options.AddArguments(settings.Arguments);

        var driver = new EdgeDriver(options);

        return driver;
    }
}