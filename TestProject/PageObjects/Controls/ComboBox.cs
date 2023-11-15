using System;
using System.Threading;
using OpenQA.Selenium;
using static TestProject.Helpers.WaitHelper;
using static TestProject.Helpers.BrowserHelper;

namespace TestProject.PageObjects.Controls
{
    public class ComboBox : BaseControl
    {
        public string ItemPath { get; set; }

        /// <summary>
        /// Return combo box text value
        /// </summary>
        public string Value => Find().GetAttribute("value");

        public ComboBox(string name, By locator, string itemXPathLocator) : base(name, locator)
        {
            ItemPath = itemXPathLocator;
        }

        /// <summary>
        /// Open, type and choose value from the list for the combobox
        /// </summary>
        /// <param name="value">value to type in the combo box</param>
        /// <param name="clear">if true - clear combo box before entering text</param>
        /// <param name="scrollTo">if true - scroll combo box to the visible area</param>
        public void Select(string value, bool clear = false, bool scrollTo = false)
        {
            OpenAndTypeValue(value, clear: clear, scrollTo: scrollTo);
            ChooseValue(value, scrollTo: scrollTo);

        }

        /// <summary>
        /// Open and type value in the combobox
        /// </summary>
        /// <param name="value">value to type in the combo box</param>
        /// <param name="clear">if true - clear combo box before entering text</param>
        /// <param name="scrollTo">if true - scroll combo box to the visible area</param>
        public void OpenAndTypeValue(string value, bool clear = false, bool scrollTo = true)
        {
            if (clear)
                Find().Clear();

            WaitUntilClickable(Locator);

            if (scrollTo)
                ScrollToView(Find());

            Find().Click();
            Find().SendKeys(value);
        }

        /// <summary>
        /// Choose value from list of combo box items
        /// </summary>
        /// <param name="value">value to choose from combo box list</param>
        /// <param name="scrollTo">if true - scroll combo box to the visible area</param>
        public void ChooseValue(string value, bool scrollTo = false)
        {
            var itemLocator = By.XPath(string.Format(ItemPath, value));
            WaitUntilClickable(itemLocator);
            var itemElement = Driver.FindElement(itemLocator);

            if (scrollTo)
                ScrollToView(itemElement);

            try
            {
                itemElement.Click();
            }
            catch (Exception e) when (e is ElementClickInterceptedException || e is StaleElementReferenceException || e is ElementNotInteractableException)
            {
                // sometimes when the list is reloaded after filtering combobox values - element click is intercepted
                Thread.Sleep(TimeSpan.FromSeconds(2));
                Driver.FindElement(itemLocator).Click();
            }
        }
    }
}
