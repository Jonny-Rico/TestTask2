using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using static TestProject.Helpers.BrowserHelper;
using static SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace TestProject.Helpers
{
    /// <summary>
    /// Helper class with methods related to wait for different WebElement conditions
    /// </summary>
    public static class WaitHelper
    {
        public static int DefaultTimeout = 10;

        /// <summary>
        /// Wait for element to be visible using locator.
        /// Throws NoSuchElementException if element is not visible.
        /// </summary>
        /// <param name="locator">locator to element</param>
        /// <param name="timeout">time to wait in seconds</param>
        public static void WaitUntilVisible(By locator, int? timeout = null)
        {
            timeout = GetValueOrDefault(timeout);
            try
            {
                var wait = new WebDriverWait(Driver, GetTimeSpan(timeout));
                wait.Until(ElementIsVisible(locator));
            }
            catch (WebDriverTimeoutException e)
            {
                throw new WebDriverTimeoutException($"'{locator}' doesn't appear in {timeout} seconds.", e);
            }
        }

        /// <summary>
        /// Wait for element to be clickable using locator.
        /// Throws NoSuchElementException if element is not clickable.
        /// </summary>
        /// <param name="locator">locator to element</param>
        /// <param name="timeout">time to wait in seconds</param>
        public static void WaitUntilClickable(By locator, int? timeout = null)
        {
            var wait = new WebDriverWait(Driver, GetTimeSpan(timeout));
            wait.Until(ElementToBeClickable(locator));
        }

        /// <summary>
        /// Wait for element to disappear using locator.
        /// </summary>
        /// <param name="locator">locator to element</param>
        /// <param name="timeout">time to wait in seconds</param>
        public static void WaitUntilDisappear(By locator, int? timeout = null)
        {
            timeout = GetValueOrDefault(timeout);
            try
            {
                var wait = new WebDriverWait(Driver, GetTimeSpan(timeout));
                wait.Until(InvisibilityOfElementLocated(locator));
            }
            catch (WebDriverTimeoutException e)
            {
                throw new WebDriverTimeoutException($"'{locator}' doesn't disappear in {timeout} seconds.", e);
            }
        }

        /// <summary>
        /// Get value or returns default wait time if the input value is null
        /// </summary>
        /// <param name="timeout">Seconds to wait int value</param>
        /// <returns>Default selenium timeout if the value is null; otherwise - input value</returns>
        public static int GetValueOrDefault(int? timeout)
        {
            return timeout.GetValueOrDefault(DefaultTimeout);
        }

        /// <summary>
        /// Convert seconds to TimeSpan object. If the input value is null - the default selenium timeout is used.
        /// </summary>
        /// <param name="timeout">Seconds to convert</param>
        /// <returns>TimeOut object representing the input seconds value</returns>
        private static TimeSpan GetTimeSpan(int? timeout)
        {
            return TimeSpan.FromSeconds(GetValueOrDefault(timeout));
        }

        /// <summary>
        /// Check for element to be visible using locator.
        /// Returns true if success. Return false if element not visible.
        /// </summary>
        /// <param name="locator">locator to element</param>
        /// <param name="timeout">time to wait in seconds</param>
        public static bool IsElementVisible(By locator, int? timeout = null)
        {
            try
            {
                WaitUntilVisible(locator, timeout);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}