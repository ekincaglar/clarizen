Feature: Api_Timesheet


Background: 
	Given I Login using credentials in appsettings
	And I remove pre-existing test data
	And I delete users with an OfficePhone Number of '020 7946 0000'
	And I wait 2 second
	And  I create the following User
	| FirstName | LastName | email                               | OfficePhone   | MobilePhone  | ExternalUser | SuperUser | Financial |
	| UnitTest  | Bloggs   | UnitTest.bloggs@CreateUserTest1.com | 020 7946 0000 | 07700 900000 | true         | false     | false     |
	| UnitTest  | Smith    | UnitTest.Smith@CreateUserTest1.com  | 020 7946 0000 | 07700 900000 | false        | false     | false      |

Scenario: CallMissingTimesheets
	Given I set the test user to be 'UnitTest.bloggs@CreateUserTest1.com'
	And I call MissingTimesheets for testuser between '<<MondayLastWeek>>' and '<<fridaylastweek>>' inclusive
	Then there are 5 missing timesheets
	Given I get the workpattern for the test user

Scenario: GetMissingWorkPatterns
	Given I set the test user to be 'UnitTest.bloggs@CreateUserTest1.com'
	And  I get the workpattern for the test user
	Then the first workday of the week is 'Monday'
	And the testUser has 20 working days a month
	And the testUsers defaultWorkingDay is
	| endHour | isWorkingDay | startHour | totalWorkingHours |
	| 16      | true         | 8         | 8                 |
	And the testusers weekDayInformation is
	| endHour | isWorkingDay | startHour | totalWorkingHours |
	| 16      | true         | 8         | 8                 |
	| 16      | true         | 8         | 8                 |
	| 16      | true         | 8         | 8                 |
	| 16      | true         | 8         | 8                 |
	| 16      | true         | 8         | 8                 |
	| 16      | true         | 8         | 8                 |
	| 16      | true         | 8         | 8                 |
