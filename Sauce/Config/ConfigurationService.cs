using Microsoft.Extensions.Configuration;

namespace Sauce.Config;

/// <summary>
/// Utility class to read configuration settings.
/// </summary>
public class ConfigurationService
{
    /// <summary>
    /// The application configuration root.
    /// </summary>
    public static readonly IConfigurationRoot Configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();

    /// <summary>
    /// Gets the browser settings from configuration.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static BrowserSettings GetBrowserSettings()
    {
        return Configuration.GetSection("BrowserSettings").Get<BrowserSettings>()
            ?? throw new InvalidOperationException("Failed to load BrowserSettings from configuration.");
    }

    /// <summary>
    /// Gets the full URL for a given page key.
    /// </summary>
    /// <returns></returns>
    public static string GetFullUrlPage(string urlKey)
    {
        var url = Configuration.GetSection($"UrlSettings:PageUrls:{urlKey}").Value;
        var baseUrl = Configuration.GetSection("UrlSettings:BaseUrl").Value;

        ArgumentNullException.ThrowIfNull(url);
        ArgumentNullException.ThrowIfNull(baseUrl);

        return baseUrl.TrimEnd('/') + url;
    }
}