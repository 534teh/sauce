using OpenQA.Selenium;
using Reqnroll;
using sauce.Config;
using sauce.Pages;
using Shouldly;

namespace sauce.StepDefinitions;

[Binding]
public class LoginSteps
{
    private readonly IWebDriver _driver;
    private readonly IConfigurationService _configService;
    private readonly LoginPage _loginPage;

    public LoginSteps(IWebDriver driver, IConfigurationService configService)
    {
        this._driver = driver;
        this._configService = configService;
        this._loginPage = new LoginPage(this._driver, this._configService);
    }


    [Given(@"I am on the login page")]
    public void GivenIAmOnTheLoginPage()
    {
        this._loginPage.Open();
    }

    [When(@"I enter username ""(.*)""")]
    public void WhenIEnterUsername(string username)
    {
        this._loginPage.EnterUsername(username);
    }

    [When(@"I enter password ""(.*)""")]
    public void WhenIEnterPassword(string password)
    {
        this._loginPage.EnterPassword(password);
    }

    [When(@"I clear the username field")]
    public void WhenIClearTheUsernameField()
    {
        this._loginPage.ClearUsername();
    }

    [When(@"I clear the password field")]
    public void WhenIClearThePasswordField()
    {
        this._loginPage.ClearPassword();
    }

    [When(@"I click the login button")]
    public void WhenIClickTheLoginButton()
    {
        this._loginPage.ClickLogin();
    }

    [Then(@"I should see the error message ""(.*)""")]
    public void ThenIShouldSeeTheErrorMessage(string expectedError)
    {
        var actualError = this._loginPage.GetErrorMessage();
        actualError.ShouldBe(expectedError);
    }

    [Then(@"I should be redirected to the inventory page")]
    public void ThenIShouldBeRedirectedToTheInventoryPage()
    {
        this._driver.Url.ShouldContain("inventory");
    }

    [Then(@"the page title should be ""(.*)""")]
    public void ThenThePageTitleShouldBe(string expectedTitle)
    {
        var inventoryPage = new InventoryPage(this._driver, this._configService);

        var actualTitle = inventoryPage.GetTitle();
        actualTitle.ShouldBe(expectedTitle);
    }
}