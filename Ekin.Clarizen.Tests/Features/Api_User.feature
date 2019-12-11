Feature: Api_User
	Test User functionality using the API
	Background: 
	Given I Login using credentials in appsettings
	And I remove pre-existing test data
	And I delete users with an OfficePhone Number of '020 7946 0000'

Scenario: CreateUsers
	Given I remove pre-existing test data
	
	And I wait 2 second
	And I create the following User
	| FirstName | LastName | email                               | OfficePhone   | MobilePhone  | ExternalUser | SuperUser | Financial |
	| UnitTest  | Bloggs   | UnitTest.bloggs@CreateUserTest1.com | 020 7946 1234 | 07700 900000 | True         | False     | False     |
	| UnitTest  | Smith    | UnitTest.Smith@CreateUserTest1.com  | 020 7946 1234 | 07700 900000 | False        | False     | True      |
	Then the following users exist with an OfficePhone Number of '020 7946 1234'
	| FirstName | LastName | email                               | OfficePhone   | MobilePhone  | ExternalUser | SuperUser | Financial | 
	| UnitTest  | Bloggs   | UnitTest.bloggs@CreateUserTest1.com | 020 7946 1234 | 07700 900000 | True         | False     | False     |
	| UnitTest  | Smith    | UnitTest.Smith@CreateUserTest1.com  | 020 7946 1234 | 07700 900000 | False        | False     | True      |

##Scenario: CreateUsersAndChangeState
##	Given I create the following User
##	| FirstName | LastName | email                               | OfficePhone   | MobilePhone  | ExternalUser | SuperUser | Financial |
##	| UnitTest  | Bloggs   | UnitTest.bloggs@CreateUserTest1.com | 020 7946 0000 | 07700 900000 | true         | false     | false     |
##	| UnitTest  | Smith    | UnitTest.Smith@CreateUserTest1.com  | 020 7946 0000 | 07700 900000 | false        | false     | true      |
##	Then the following users exist with an OfficePhone Number of '020 7946 0000'
##	| FirstName | LastName | email                               | OfficePhone   | MobilePhone  | ExternalUser | SuperUser | Financial |
##	| UnitTest  | Bloggs   | UnitTest.bloggs@CreateUserTest1.com | 020 7946 0000 | 07700 900000 | true         | false     | false     |
##	| UnitTest  | Smith    | UnitTest.Smith@CreateUserTest1.com  | 020 7946 0000 | 07700 900000 | false        | false     | true      |
##	Given I change the state to 'Suspended' for user by email 'UnitTest.bloggs@CreateUserTest1.com'
##  Then the user by email 'UnitTest.bloggs@CreateUserTest1.com' is in state 'Suspended'