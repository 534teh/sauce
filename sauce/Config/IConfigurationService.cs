namespace sauce.Config;

public interface IConfigurationService
{
    BrowserSettings GetBrowserSettings();
    string GetFullUrlPage(string urlKey);
}