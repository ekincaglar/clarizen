﻿Feature: Api_User
	Test User functionality using the API
	Background: 
	Given I Login using credentials in appsettings
	And I remove pre-existing test data

Scenario: There is 1 admin user
	Then there are 1 admin users