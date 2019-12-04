Feature: TestHelper
	Test the TestHelper methods

@mytag
Scenario: TestHelper.convertToDateTime1
	Given When I set the TimeProvider date to '6 Nov 2019 13:34:56'
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

Scenario: TestHelper.convertToDateTime2
	Given When I set the TimeProvider date to '4 Feb 2020'
	Given I TestHelper function convertToDateTime with the following
	| Value                    | Result              | IncludeTime |
	| 1 Feb 2000               | 1 Feb 2000          | false       |
	| December 1 2222          | 1 Dec 2222          | false       |
	| 1 Feb 2000 13:34:43      | 1 Feb 2000 13:34:43 | true        |
	| December 1 2222 09:23:48 | 1 Dec 2222 09:23:48 | true        |
	| <<now>>                  | 4 Feb 2020 13:34:56 | true        |
	| <<today>>                | 4 Feb 2020           | false       |
	| <<yesterday>>            | 3 Feb 2020           | false       |
	| <<yearstart>>            | 1 Jan 2020           | false       |
	| <<monthstart>>           | 1 Feb 2019           | false       |
	| <<mondaylastweek>>       | 27 Jan 2019          | false       |
	| <<fridaylastweek>>       | 31 Jan 2019          | false       |
