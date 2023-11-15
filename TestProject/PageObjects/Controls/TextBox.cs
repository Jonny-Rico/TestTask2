using OpenQA.Selenium;

namespace TestProject.PageObjects.Controls
{
    /// <summary>
    /// Class for methods and elements of TextBox control
    /// </summary>
    public class TextBox : BaseControl
    {
        private CustomControl AmountTextBoxWrapper => new("Amount TextBox Wrapper", By.XPath("//div[contains(@class,'amount-input__Wrapper')]"));
        public TextBox(string name, By locator) : base(name, locator)
        {

        }

        public void EnterText(string text)
        {
            WaitForVisible();
            AmountTextBoxWrapper.Click();
            Find().SendKeys(text);
        }

        public void ClearText()
        {
            WaitForClickable();
            AmountTextBoxWrapper.Click();
            Find().SendKeys(Keys.Control + "a");
            Find().SendKeys(Keys.Delete);
        }
    }
}
