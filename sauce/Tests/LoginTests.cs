using sauce.Base;

namespace sauce.Tests
{
    [TestClass]
    public class LoginTests : BaseTest
    {
        [DataTestMethod]
        [DataRow("", "", "Epic sadface: Username is required")]
        public void UC1_EmptyCredentialsTest(string username, string password, string expectedError)
        {
            log.Info($"Running UC1 with username: '{username}', password: '{password}'");
            LoginPage!.EnterUsername(username);
            LoginPage.EnterPassword(password);
            LoginPage.ClickLogin();
            Assert.AreEqual(expectedError, LoginPage.GetErrorMessage());
            log.Info("UC1 finished.");
        }

        [DataTestMethod]
        [DataRow("standard_user", "", "Epic sadface: Password is required")]
        public void UC2_EmptyPasswordTest(string username, string password, string expectedError)
        {
            log.Info($"Running UC2 with username: '{username}', password: '{password}'");
            LoginPage!.EnterUsername(username);
            LoginPage.EnterPassword(password);
            LoginPage.ClickLogin();
            Assert.AreEqual(expectedError, LoginPage.GetErrorMessage());
            log.Info("UC2 finished.");
        }

        [DataTestMethod]
        [DataRow("standard_user", "secret_sauce", "Products")]
        public void UC3_ValidCredentialsTest(string username, string password, string expectedTitle)
        {
            log.Info($"Running UC3 with username: '{username}', password: '{password}'");
            LoginPage!.EnterUsername(username);
            LoginPage.EnterPassword(password);
            LoginPage.ClickLogin();
            Assert.AreEqual(expectedTitle, InventoryPage!.GetTitle());
            log.Info("UC3 finished.");
        }
    }
}
