Feature: Api_ProjectTasks
	Test the projects and tasks

Background: 
	Given I Login using credentials in appsettings
	And I remove pre-existing test data

Scenario: CreateProjectWithTask
	Given I create a project.  
	And I add the following tasks to the project
	| name                       |
	| 04 A new hope              |
	| 05 The Empire Strikes Back |
	| 06 The Revenge of the Jedi |
	| 06 The Return of the Jedi  |
	And I wait 1 second
	Then the following tasks exist in the project
	| name                       |
	| 04 A new hope              |
	| 05 The Empire Strikes Back |
	| 06 The Revenge of the Jedi |
	| 06 The Return of the Jedi  |
