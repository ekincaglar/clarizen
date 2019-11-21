Feature: Api_Login
	Login Tests

@mytag
Scenario: GoodLogin
	Given I Login using credentials in appsettings

Scenario: BadLogin
	Given I Login using login 'sa' and password 'password'

