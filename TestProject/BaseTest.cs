using NUnit.Framework;
using OpenQA.Selenium;
using TestProject.PageObjects;
using TestProject.PageObjects.Controls;
using static TestProject.Helpers.BrowserHelper;

namespace TestProject
{
    public class BaseTest
    {
        public CurrencyConverterPage CurrencyConverterPage => new();

        [SetUp]
        public static void BeforeScenarioRun()
        {
            OpenBrowser();
            ClearBrowserCookies();
            GoToUrl(MainUrl);
            WaitForPageStateComplete(5);

            var acceptButton = new Button("Accept button", By.XPath("//button[.='Accept']"));
            if (acceptButton.IsVisible(3))
                acceptButton.Click();

            WaitForPageStateComplete(3);
        }

        [TearDown]
        public static void AfterScenarioRun()
        {
            CloseBrowser();
        }


    }
}