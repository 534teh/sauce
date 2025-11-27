Feature: Login Functionality
    As a user of Swag Labs
    I want to be able to login
    So that I can buy items

Background:
    Given I am on the login page

Scenario: Login fails when the username is missing
    When I click the login button
    Then I should see the error message "Epic sadface: Username is required"

Scenario: Login fails when the password is missing
    When I enter username "Username1"
    And I click the login button
    Then I should see the error message "Epic sadface: Password is required"

Scenario Outline: Login fails with invalid or locked out credentials
    When I enter username "<username>"
    And I enter password "<password>"
    And I click the login button
    Then I should see the error message "<expectedError>"

    Examples:
      | username        | password       | expectedError                                                             |
      | locked_out_user | secret_sauce   | Epic sadface: Sorry, this user has been locked out.                       |
      | invalid_user    | wrong_password | Epic sadface: Username and password do not match any user in this service |
      | standard_user   | wrong_password | Epic sadface: Username and password do not match any user in this service |
      | invalid_user    | secret_sauce   | Epic sadface: Username and password do not match any user in this service |

Scenario Outline: Valid users are redirected to the inventory page upon login
    When I enter username "<username>"
    And I enter password "<password>"
    And I click the login button
    Then I should be redirected to the inventory page
    And the page title should be "<expectedTitle>"

    Examples:
      | username                | password     | expectedTitle |
      | standard_user           | secret_sauce | Swag Labs     |
      | problem_user            | secret_sauce | Swag Labs     |
      | performance_glitch_user | secret_sauce | Swag Labs     |
      | visual_user             | secret_sauce | Swag Labs     |
      | error_user              | secret_sauce | Swag Labs     |