# Task Description

**Launch URL:** <https://www.saucedemo.com/>

**UC-1 Test Login form with empty credentials:**

1. Type any credentials into "Username" and "Password" fields.
2. Clear the inputs.
3. Hit the "Login" button.
4. Check the error messages: "Username is required".

**UC-2 Test Login form with credentials by passing Username:**

1. Type any credentials in username.
2. Enter password.
3. Clear the "Password" input.
4. Hit the "Login" button.
5. Check the error messages: "Password is required".

**UC-3 Test Login form with credentials by passing Username & Password:**

1. Type credentials in username which are under Accepted username are sections.
2. Enter password as secret sauce.
3. Click on Login and validate the title “Swag Labs” in the dashboard.

**Requirements:**

* Provide parallel execution.
* Add logging for tests.
* Use Data Provider to parametrize tests.
* Make sure that all tasks are supported by these 3 conditions: UC-1; UC-2; UC-3.

**To perform the task use the various of additional options:**

* **Test Automation tool:** Selenium WebDriver
* **Browsers:** 1) Firefox; 2) Edge
* **Locators:** CSS
* **Test Runner:** MSTest
* **Assertions:** FluentAssertions
* **[Optional] Patterns:** 1) Singleton 2) Adapter 3) Strategy
* **[Optional] Test automation approach:** BDD
* **[Optional] Loggers:** Serilog

**Criteria for successful completion:**

* **[Optional] Patterns:** 1) Factory 2) Builder 3) Decorator
* **[Optional] Test automation approach:** BDD
* **[Optional] Loggers:** Log4Net
