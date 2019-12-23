Feature: TestHelper
	Test the TestHelper methods

Background: 
Given I reset the TimeProvider
Scenario: TestHelper.convertToDateTime1
	Given I set the TimeProvider date to '6 Nov 2019 13:34:56'
	Given I TestHelper function convertToDateTime with the following
	| Value                    | Result              | IncludeTime |
	| 1 Feb 2000               | 1 Feb 2000          | false       |
	| December 1 2222          | 1 Dec 2222          | false       |
	| 1 Feb 2000 13:34:43      | 1 Feb 2000 13:34:43 | true        |
	| December 1 2222 09:23:48 | 1 Dec 2222 09:23:48 | true        |
	| <<now>>                  | 6 Nov 2019 13:34:56 | true        |
	| <<today>>                | 6 Nov 2019          | false       |
	| <<yesterday>>            | 5 Nov 2019          | false       |
	| <<yearstart>>            | 1 Jan 2019          | false       |
	| <<monthstart>>           | 1 Nov 2019          | false       |
	| <<mondaylastweek>>       | 28 Oct 2019         | false       |
	| <<fridaylastweek>>       | 1 Nov 2019          | false       |
	| <<tomorrow>>             | 7 Nov 2019          | false       |
	| <<mondaynextweek>>       | 11 Nov 2019         | false       |
	| <<fridaynextweek>>       | 15 Nov 2019         | false       |

Scenario: TestHelper.convertToDateTime2
	Given I reset the TimeProvider
	And I set the TimeProvider date to '4 Feb 2020 23:14:59'
	Given I TestHelper function convertToDateTime with the following
	| Value                    | Result              | IncludeTime |
	| 1 Feb 2000               | 1 Feb 2000          | false       |
	| December 1 2222          | 1 Dec 2222          | false       |
	| 1 Feb 2000 13:34:43      | 1 Feb 2000 13:34:43 | true        |
	| December 1 2222 09:23:48 | 1 Dec 2222 09:23:48 | true        |
	| <<now>>                  | 4 Feb 2020 23:14:59 | true        |
	| <<today>>                | 4 Feb 2020          | false       |
	| <<yesterday>>            | 3 Feb 2020          | false       |
	| <<yearstart>>            | 1 Jan 2020          | false       |
	| <<monthstart>>           | 1 Feb 2020          | false       |
	| <<mondaylastweek>>       | 27 Jan 2020         | false       |
	| <<fridaylastweek>>       | 31 Jan 2020         | false       |
	| <<tomorrow>>             | 5 Feb 2020          | false       |
	| <<mondaynextweek>>       | 10 Feb 2020         | false       |
	| <<fridaynextweek>>       | 14 Feb 2020         | false       |
