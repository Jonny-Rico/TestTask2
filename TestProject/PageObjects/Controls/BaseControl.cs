using System;
using OpenQA.Selenium;
using static TestProject.Helpers.BrowserHelper;
using static TestProject.Helpers.WaitHelper;

namespace TestProject.PageObjects.Controls
{
    /// <summary>
    /// Abstract class of page controller
    /// /// </summary>
    public abstract class BaseControl : IWebControl
    {
        public string Name { get; }
        public By Locator;

        protected BaseControl(string name, By locator)
        {
            Name = name;
            Locator = locator;
        }

        public IWebElement Find()
        {
            try
            {
                return Driver.FindElement(Locator);
            }
            catch (NoSuchElementException e)
            {
                throw new Exception($"{this} not found on page.", e);
            }
        }

        public IWebControl Click()
        {
            Find().Click();

            return this;
        }

        public void WaitForVisible(int? timeout = null)
        {
            try
            {
                WaitUntilVisible(Locator, timeout: timeout);
            }
            catch (Exception e)
            {
                throw new ElementNotVisibleException($"{this} not visible on page in {timeout} seconds.", e);
            }
        }

        public virtual void WaitForClickable(int? timeout = null)
        {
            timeout = GetValueOrDefault(timeout);

            try
            {
                WaitUntilClickable(Locator, timeout: timeout);
            }
            catch (Exception e)
            {
                throw new ElementNotInteractableException($"{this} not clickable in {timeout} seconds.", e);
            }
        }

        public void WaitForDisappear(int? timeout = null)
        {
            timeout = GetValueOrDefault(timeout);

            try
            {
                WaitUntilDisappear(Locator, timeout: timeout);
            }
            catch (Exception e)
            {
                throw new Exception($"{this} still present on page in {GetValueOrDefault(timeout)} seconds.", e);
            }
        }

        public bool IsVisible(int timeout = 5)
        {
            return IsElementVisible(Locator, timeout);
        }

        public virtual string GetText()
        {
            WaitForVisible();

            return Find().GetAttribute("innerText");
        }
    }
}
