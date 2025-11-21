namespace sauce.Config;

/// <summary>
/// Settings for a specific browser.
/// </summary>
public class BrowserSpecificSettings
{
    /// <summary>
    /// Name of the browser
    /// </summary>
    public required string BrowserName { get; set; }
    /// <summary>
    /// List of arguments to pass to the browser on startup.
    /// </summary>
    public List<string> Arguments { get; set; } = [];
}

/// <summary>
/// Global browser settings.
/// </summary>
public class BrowserSettings
{
    /// <summary>
    /// The fallback browser key to use if no environment variable is set.
    /// </summary>
    public required string FallbackBrowser { get; set; }
    /// <summary>
    /// Dictionary of browser keys to their specific settings.
    /// </summary>
    public required Dictionary<string, BrowserSpecificSettings> Browsers { get; set; }
}