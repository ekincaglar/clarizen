Feature: Api_User
	Test User functionality using the API
	Background: 
	Given I Login using credentials in appsettings
	And I remove pre-existing test data
	And I delete users with an OfficePhone Number of '020 7946 0000'

Scenario: CreateUsers
Given I create the following User
| FirstName | LastName | email                               | OfficePhone   | MobilePhone  | ExternalUser | SuperUser | Financial |
| UnitTest  | Bloggs   | UnitTest.bloggs@CreateUserTest1.com | 020 7946 0000 | 07700 900000 | true         | false     | false     |
| UnitTest  | Smith    | UnitTest.Smith@CreateUserTest1.com  | 020 7946 0000 | 07700 900000 | false        | false     | true      |
Then the following users exist with an OfficePhone Number of '020 7946 0000'
| FirstName | LastName | email                               | OfficePhone   | MobilePhone  | ExternalUser | SuperUser | Financial |
| UnitTest  | Bloggs   | UnitTest.bloggs@CreateUserTest1.com | 020 7946 0000 | 07700 900000 | true         | false     | false     |
| UnitTest  | Smith    | UnitTest.Smith@CreateUserTest1.com  | 020 7946 0000 | 07700 900000 | false        | false     | true      |


