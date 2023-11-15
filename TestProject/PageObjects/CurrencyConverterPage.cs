using System;
using OpenQA.Selenium;
using TestProject.PageObjects.Controls;

namespace TestProject.PageObjects
{
    public class CurrencyConverterPage: BasePage
    {
        public TextBox AmountTextBox => new("Amount textbox", By.Id("amount"));

        public ComboBox FromComboBox => new("From ComboBox", By.XPath("//input[@id='midmarketFromCurrency']"),
            "//*[@id='midmarketFromCurrency-listbox']//li[@role='option' and .//span[@class='description'][.='{0}']]");
        public CustomControl SelectedSourceCurrency => new("Selected Source Currency", By.Id("midmarketFromCurrency-descriptiveText"));
        public ComboBox ToComboBox => new("To ComboBox", By.XPath("//input[@id='midmarketToCurrency']"),
            "//*[@id='midmarketToCurrency-listbox']//li[@role='option' and .//span[@class='description'][.='{0}']]");
        public CustomControl SelectedTargetCurrency => new("Selected Target Currency", By.Id("midmarketToCurrency-descriptiveText"));

        public Button SwapButton => new("Swap currencies button", By.XPath("//button[@aria-label='Swap currencies']"));
        public Button ConvertButton => new("Convert currencies button", By.XPath("//button[.='Convert']"));

        public Label ResultAmountLabel => new("Amount to convert label", By.XPath("//*[contains(@class,'result__ConvertedText')]"));
        public Label ResultConvertedValueLabel => new("Converted value label", By.XPath("//*[contains(@class,'result__BigRate')]"));
        public Label UnitRatesLabel => new("Unit rates value label", By.XPath("//*[contains(@class,'unit-rates')]/p[1]"));
        public Label UnitRatesReverseLabel => new("Unit reverse rates value label", By.XPath("//*[contains(@class,'unit-rates')]/p[2]"));
        public Label ErrorMessageLabel => new("Error message label", By.XPath("//*[contains(@class,'currency-converter__ErrorText')]"));
        public CustomControl ConversionSpinner => new("Conversion Spinner", By.XPath("//*[contains(@class, 'preloader__Shpin')]"));

        public void ConvertCurrencies(string amount, string sourceCurrencyName, string targetCurrencyName)
        {
            SetAmount(amount);
            SelectSourceCurrency(sourceCurrencyName);
            SelectTargetCurrency(targetCurrencyName);
            ConvertButton.Click();
            UnitRatesReverseLabel.WaitForVisible(5);
        }

        public void ReenteringAmount(string amount)
        {
            AmountTextBox.ClearText();
            SetAmount(amount);
            ConversionSpinner.WaitForVisible(1);
            ConversionSpinner.WaitForDisappear(3);
        }

        public void SwapConversion()
        {
            SwapButton.Click();
            UnitRatesReverseLabel.WaitForVisible(5);
        }

        public void SetAmount(string amount)
        {
            AmountTextBox.EnterText(amount);
        }

        public void SelectSourceCurrency(string currencyName)
        {
            FromComboBox.Select(currencyName);
        }

        public void SelectTargetCurrency(string currencyName)
        {
            ToComboBox.Select(currencyName);
        }

        public double GetConvertedAmount()
        {
            var text = ResultAmountLabel.GetText();
            var result = text.Remove(text.IndexOf(" ", StringComparison.Ordinal));

            //convert to double with specific accuracy
            var unitRate = Math.Round(double.Parse(result), 2);

            return unitRate;
        }

        public double GetConvertedResult()
        {
            var text = ResultConvertedValueLabel.GetText();
            var result = text.Remove(text.IndexOf(" ", StringComparison.Ordinal));

            //convert to double with specific accuracy
            var unitRate = Math.Round(double.Parse(result), GetStringValueAccuracy(result));

            return unitRate;
        }
        
        public double GetUnitRate()
        {
            var value = UnitRatesLabel.GetText().Remove(0, 8);
            var rate = value.Remove(value.IndexOf(" ", StringComparison.Ordinal));

            //convert to double with specific accuracy
            var unitRate = Math.Round(double.Parse(rate), GetStringValueAccuracy(rate));

            return unitRate;
        }

        public double GetReverseUnitRate()
        {
            var value = UnitRatesReverseLabel.GetText().Remove(0, 8);
            var rate = value.Remove(value.IndexOf(" ", StringComparison.Ordinal));

            //convert to double with specific accuracy
            var unitReverseRate = Math.Round(double.Parse(rate), GetStringValueAccuracy(rate));

            return unitReverseRate;
        }

        /// <summary>
        /// Get specific accuracy of number's value
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Number of digits after comma</returns>
        public int GetStringValueAccuracy(string value)
        {
            return value.Length - value.IndexOf(".", StringComparison.Ordinal) - 1;
        }
    }
}
