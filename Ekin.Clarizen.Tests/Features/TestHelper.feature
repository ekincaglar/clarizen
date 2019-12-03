Feature: TestHelper
	Test the TestHelper methods

@mytag
Scenario: TestHelper.convertToDateTime
	Given When I set the TimeProvider date to '2 Nov 2019'
	Given I TestHelper function convertToDateTime with the following
	| Value                    | Result              | IncludeTime |
	| 1 Feb 2000               | 1 Feb 2000          | false       |
	| December 1 2222          | 1 Dec 2222          | false       |
	| 1 Feb 2000 13:34:43      | 1 Feb 2000 13:34:43 | true        |
	| December 1 2222 09:23:48 | 1 Dec 2222 09:23:48 | true        |