Feature: Api_Timesheet


Background: 
	Given I Login using credentials in appsettings
	And I remove pre-existing test data
	And I delete users with an OfficePhone Number of '020 7946 0000'
	And  I create the following User
	| FirstName | LastName | email                               | OfficePhone   | MobilePhone  | ExternalUser | SuperUser | Financial |
	| UnitTest  | Bloggs   | UnitTest.bloggs@CreateUserTest1.com | 020 7946 0000 | 07700 900000 | true         | false     | false     |
	| UnitTest  | Smith    | UnitTest.Smith@CreateUserTest1.com  | 020 7946 0000 | 07700 900000 | false        | false     | true      |
Scenario: CallMissingTimesheets
	Given I call MissingTimesheets for user by email 'UnitTest.bloggs@CreateUserTest1.com' between '<<MondayLastWeek>>' and '<<fridaylastweek>>'
