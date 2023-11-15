
Used IDE: Visual Studio 2022
Installed packages:
- coverlet.collector
- DotNetSeleniumExtras.WaitHelpers
- Microsoft.NET.Test.Sdk
- NUnit
- NUnit3TestAdapter
- Selenium.WebDriver
- Selenium.WebDriver.ChromeDriver


Here are my comments for this test-task.

- Source and target combobox filtering contains extra options after filtering by currency alphabetical code.
- Maximum and minimum range of converted amount is not specified in reqiurements
- Separator for decimal numeric format is not specified in reqiurements
- AC4 is inconsistent: Accuracy of converted result depends on amount of digits, in the same time unit rate has specific accuracy.
  In my opinion converted result should be validated with accuracy "2" according to real currency minimal value (0.01).
- AC5 should be updated with specified list of most popular currencies. Cannot be covered by automation before clarification
- AC6 is inconsistent. There is 2 way to convert: first attempt with clicking "convert button" and "reentering" new value after first conversion succeed.
  There should be 2 different requirements: unable to convert first time and success conversion with error for reentering amount. Should be clarified. Error message should be specified
- AC7: values  that contains non-numeric symbols at the end of some numeric symbols doesn't cause errors, that should be clarified (maybe it is a bug)

