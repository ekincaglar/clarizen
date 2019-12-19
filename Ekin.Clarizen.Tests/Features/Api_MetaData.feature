Feature: MetaDataTests
	Test the metadata Calls

Background: 
	Given I Login using credentials in appsettings

Scenario:  Describe the weekDays entity
	Given I call the 'weekdays' entity
	Then there are fields in the entity description
