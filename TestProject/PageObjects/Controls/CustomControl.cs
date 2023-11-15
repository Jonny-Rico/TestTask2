using OpenQA.Selenium;

namespace TestProject.PageObjects.Controls
{
    public class CustomControl : BaseControl
    {
        public CustomControl(string name, By locator) : base(name, locator)
        {
        }
    }
}
