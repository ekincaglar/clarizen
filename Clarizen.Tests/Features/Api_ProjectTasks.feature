Feature: Api_ProjectTasks
	Test the projects and tasks

Background: 
	Given I Login using credentials in appsettings
	And I remove pre-existing test data

Scenario: CreateProjectWithTask
	Given I create a project.  
	And I add task '04 A new hope' for today only
	And I add task '05 The Empire Strikes Back' for today only
	And I add task '06 The Revenge of the Jedi' for today only
	And I add task '06 The Return of the Jedi' for today only
