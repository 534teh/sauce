using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;

namespace sauce.Drivers;

public class WebDriverManager
{
    private static readonly ThreadLocal<IWebDriver?> _driver = new(() =>
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
    });

    private WebDriverManager() { }

    public static IWebDriver Driver => _driver.Value!;

    public static void QuitDriver()
    {
        if (_driver.IsValueCreated && _driver.Value != null)
        {
            _driver.Value.Quit();
        }
    }
}
