﻿Feature: Extenstions
	Test the extentions


Background: 
Given I reset the TimeProvider
And I wait 2 second

Scenario: StartOfWeek
Given I Test Extention Method StartOfWeek with the following values
| TargetDate  | DayOfWeek | Expected    |
| 25 Nov 2019 | Monday    | 25 Nov 2019 |
| 25 Nov 2019 | Tuesday   | 26 Nov 2019 |
| 25 Nov 2019 | Wednesday | 27 Nov 2019 |
| 25 Nov 2019 | Thursday  | 28 Nov 2019 |
| 25 Nov 2019 | Friday    | 29 Nov 2019 |
| 25 Nov 2019 | Saturday  | 30 Nov 2019 |
| 25 Nov 2019 | Sunday    | 01 Dec 2019 |
| 07 Mar 2016 | Monday    | 07 Mar 2016 |
| 07 Mar 2016 | Tuesday   | 08 Mar 2016 |
| 07 Mar 2016 | Wednesday | 09 Mar 2016 |
| 07 Mar 2016 | Thursday  | 10 Mar 2016 |
| 07 Mar 2016 | Friday    | 11 Mar 2016 |
| 07 Mar 2016 | Saturday  | 12 Mar 2016 |
| 07 Mar 2016 | Sunday    | 13 Mar 2016 |
| 01 Mar 2012 | Monday    | 27 Feb 2012 |
| 01 Mar 2012 | Tuesday   | 28 Feb 2012 |
| 01 Mar 2012 | Wednesday | 29 Feb 2012 |
| 01 Mar 2012 | Thursday  | 01 Mar 2012 |
| 01 Mar 2012 | Friday    | 02 Mar 2012 |
| 01 Mar 2012 | Saturday  | 03 Mar 2012 |
| 01 Mar 2012 | Sunday    | 04 Mar 2012 |


Scenario: GetDayInWeek1
Given I set the TimeProvider date to '6 Nov 2019 13:34:56'
And I wait 2 second
Then I check extension method GetDayInWeek returns the following
| Value     | Expected    |
| Monday    | 4 Nov 2019  |
| Tuesday   | 5 Nov 2019  |
| Wednesday | 6 Nov 2019  |
| Thursday  | 7 Nov 2019  |
| Friday    | 8 Nov 2019  |
| Saturday  | 9 Nov 2019  |
| Sunday    | 10 Nov 2019 |


Scenario: GetDayInWeek2
Given I set the TimeProvider date to '1 Mar 2012 13:34:56'
And I wait 2 second
Then I check extension method GetDayInWeek returns the following
| Value     | Expected    |
| Monday    | 27 Feb 2012 |
| Tuesday   | 28 Feb 2012 |
| Wednesday | 29 Feb 2012 |
| Thursday  | 1 Mar 2012  |
| Friday    | 2 Mar 2012  |
| Saturday  | 3 Mar 2012  |
| Sunday    | 4 Mar 2012  |


Scenario: GetFirstDayOfWeek
Given I set the TimeProvider date to '29 Feb 2012 13:34:56'
And I wait 2 second
Given I check the date time extenstion GetFirstDayOfWeek
| Value       | Expected    |
| 1 Mar 2012  | 27 Feb 2012 |
| 2 Mar 2012  | 27 Feb 2012 |
| 3 Mar 2012  | 27 Feb 2012 |
| 4 Mar 2012  | 27 Feb 2012 |
| 27 Feb 2012 | 27 Feb 2012 |
| 28 Feb 2012 | 27 Feb 2012 |
| 29 Feb 2012 | 27 Feb 2012 |

