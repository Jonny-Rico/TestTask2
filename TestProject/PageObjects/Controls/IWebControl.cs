namespace TestProject.PageObjects.Controls
{
    /// <summary>
    /// Interface to implement common UI control's methods
    /// </summary>
    public interface IWebControl
    {
        IWebControl Click();

        void WaitForVisible(int? timeout = null);
    }
}
