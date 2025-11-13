using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;

namespace sauce.Drivers;

/// <summary>
/// Factory class to create WebDriver instances based on environment configuration.
/// </summary>
public class WebDriverCreator
{
    private WebDriverCreator() { }

    /// <summary>
    /// Creates a new IWebDriver instance based on the BROWSER environment variable.
    /// </summary>
    /// <returns>IWebDriver instance.</returns>
    public static IWebDriver Create()
    {
        var browser = Environment.GetEnvironmentVariable("BROWSER")?.ToLower() ?? "chrome";
        switch (browser)
        {
            case "firefox":
                var firefoxOptions = new FirefoxOptions();
                firefoxOptions.AddArguments(
                    "--headless",
                    "--disable-gpu",
                    "--width=1920",
                    "--height=1080"
                );

                return new FirefoxDriver(firefoxOptions);

            case "edge":
                var edgeOptions = new EdgeOptions();
                edgeOptions.AddArguments(
                    "--headless",
                    "--disable-gpu",
                    "--window-size=1920,1080"
                );

                return new EdgeDriver(edgeOptions);

            case "chrome":
            default:
                var chromeOptions = new ChromeOptions();
                chromeOptions.AddArguments(
                    "--headless",
                    "--no-sandbox",
                    "--disable-gpu",
                    "--disable-dev-shm-usage",
                    "window-size=1920,1080"
                );

                var service = ChromeDriverService.CreateDefaultService();
                return new ChromeDriver(service, chromeOptions);
        }
    }
}
