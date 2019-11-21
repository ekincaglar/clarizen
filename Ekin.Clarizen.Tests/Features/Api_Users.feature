Feature: Api_Users
	Test the creation of users in a project

Background: 
Given I Login using credentials in appsettings

Scenario: CreateUser
Given I create the following User
| FirstName | LastName | email                | Username      | JobTitle | OfficePhone   | MobilePhone | DirectManager | Admin | ExternalUser | SuperUser | FinancialPermissions |
| Joe       | Bloggs   | joe.bloggs@dummy.com | joe.Bloggs345 | Air      | 020 7946 0000 | 7700 900000 |               | true  | true         | false     | false                |