Feature: Extenstions
	Test the extentions

@mytag
Scenario: StartOfWeek
Given I Test Extention Method StartOfWeek with the following values
| TargetDate    | DayOfWeek | Expected    |
| 25 Nov 2019 | Monday    | 25 Nov 2019 |
| 25 Nov 2019 | Tuesday   | 26 Nov 2019 |
| 25 Nov 2019 | Wednesday | 27 Nov 2019 |
| 25 Nov 2019 | Thursday  | 28 Nov 2019 |
| 25 Nov 2019 | Friday    | 29 Nov 2019 |
| 25 Nov 2019 | Saturday  | 30 Nov 2019 |
| 25 Nov 2019 | Sunday    | 01 Dec 2019 |
