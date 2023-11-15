using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace TestProject.Helpers
{
    /// <summary>
    /// Class to initialize webdriver and open/configure/close browser.
    /// </summary>
    public static class BrowserHelper
    {
        [ThreadStatic]
        private static IWebDriver? driver;

        public static readonly string MainUrl = "https://www.xe.com/";
        public static IWebDriver Driver
        {
            get => driver;
            private set => driver = value;
        }

        public static void OpenBrowser()
        {
            try
            {
                Driver = new ChromeDriver(AppDomain.CurrentDomain.BaseDirectory, SetChromeOptions());
            }
            catch (Exception)
            {
                Thread.Sleep(5000);
                Driver = new ChromeDriver(SetChromeOptions());
            }

            Driver.Manage().Timeouts().ImplicitWait.Add(TimeSpan.FromSeconds(10));
            Driver.Manage().Window.Maximize();
        }

        /// <summary>
        /// Set Chrome Options
        /// </summary>
        /// <returns>Set of chrome options</returns>
        private static ChromeOptions SetChromeOptions()
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--no-sandbox");

            return chromeOptions;
        }

        public static void CloseBrowser()
        {
            Driver.Quit();
            Driver.Dispose();
            Driver = null;
        }

        public static void ClearBrowserCookies()
        {
            Driver.Manage().Cookies.DeleteAllCookies();
        }

        public static void GoToUrl(string url)
        {
            Driver.Navigate().GoToUrl(url);
        }

        public static string GetUrl() => Driver.Url;

        public static void WaitForPageStateComplete(int timeout = 20)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeout));
            wait.Until(d =>
                ((IJavaScriptExecutor)Driver).ExecuteScript("return document.readyState").Equals("complete"));
        }

        public static void ScrollToView(IWebElement element)
        {
            //this method will use javasript executor to move page to element specified
            var js = (IJavaScriptExecutor)Driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }
    }
}