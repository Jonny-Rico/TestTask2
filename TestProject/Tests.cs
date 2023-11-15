using System;
using NUnit.Framework;
using TestProject.Helpers;

namespace TestProject
{
    public class Tests : BaseTest
    {
        [Test(Description = "AC1, AC3")]
        [TestCase("US Dollar", "Euro", "100")]
        public void ConvertCurrency(string source, string target, string amount)
        {
            CurrencyConverterPage.ConvertCurrencies(amount, source, target);

            Assert.IsTrue(CurrencyConverterPage.ResultAmountLabel.IsVisible(), "Result doesn't contain amount to conversion");
            Assert.IsTrue(CurrencyConverterPage.ResultConvertedValueLabel.IsVisible(), "Conversion result is not visible");
            Assert.IsTrue(CurrencyConverterPage.UnitRatesLabel.IsVisible(), "Unit rates is not visible");
            Assert.IsTrue(CurrencyConverterPage.UnitRatesReverseLabel.IsVisible(), "Reverse unit rates is not visible");
        }

        [Test(Description = "AC2")]
        [TestCase("US Dollar", "Euro", "0.01")] // minimum possible amount
        [TestCase("US Dollar", "Euro", "9223372036854775807")] //long int
        [TestCase("US Dollar", "Euro", "255.5")] // decimal value
        [TestCase("US Dollar", "Euro", "255.50")] // decimal value
        [TestCase("US Dollar", "Euro", "255.500")] // decimal value
        [TestCase("US Dollar", "Euro", "33,01")] //different separator of decimal format
        public void NumericAmountShouldBeAccepted(string source, string target, string amount)
        {
            CurrencyConverterPage.ConvertCurrencies(amount, source, target);

            Assert.IsTrue(CurrencyConverterPage.ResultConvertedValueLabel.IsVisible(), "Conversion result is not visible");
        }

        [Test(Description = "AC4")]
        [TestCase("US Dollar", "Euro", "10")]
        public void ConversionShouldBeCorrect(string source, string target, string amount)
        {
            CurrencyConverterPage.ConvertCurrencies(amount, source, target);

            //Get results
            var rate = CurrencyConverterPage.GetUnitRate();
            var reverseRate = CurrencyConverterPage.GetReverseUnitRate();
            var convertedResult = Math.Round(CurrencyConverterPage.GetConvertedResult(), 2);
            var amountResult = Math.Round(double.Parse(amount), 2);

            //Round values due to inconsistent acceptance criteria
            var mathResult = Math.Round( amountResult * rate, 2);
            Assert.AreEqual(convertedResult, mathResult, "Conversion was incorrect");

            mathResult = Math.Round(amountResult / reverseRate, 2);
            Assert.AreEqual(convertedResult, mathResult, "Conversion was incorrect");
        }

        [Test(Description = "AC6")]
        [TestCase("US Dollar", "Euro", "-10")]
        public void ReenteringNegativeAmountValueShouldBeConvertedWithWarning(string source, string target, string amount)
        {
            var errorMessage = "Please enter an amount greater than 0";

            CurrencyConverterPage.ConvertCurrencies("10", source, target);
            CurrencyConverterPage.ResultConvertedValueLabel.WaitForVisible(5);
            CurrencyConverterPage.ReenteringAmount(amount);

            Assert.IsTrue(CurrencyConverterPage.ErrorMessageLabel.IsVisible(1), "Error message didn't appear");
            Assert.AreEqual(CurrencyConverterPage.ErrorMessageLabel.GetText(), errorMessage, "Error message differs from expected");
            Assert.IsTrue(CurrencyConverterPage.ResultConvertedValueLabel.IsVisible(), "Conversion result is not visible");
        }

        [Test(Description = "AC7")]
        [TestCase("A")]
        [TestCase("@")]
        [TestCase("*")]
        [TestCase("a123")]
        [TestCase("123a")]
        [TestCase("123*")]
        [TestCase("()")]
        [TestCase(" ")]
        public void NonNumericAmountValueShouldCauseError(string amount)
        {
            var errorMessage = "Please enter a valid amount";

            CurrencyConverterPage.SetAmount(amount);

            Assert.IsTrue(CurrencyConverterPage.ErrorMessageLabel.IsVisible(1), "Error message didn't appear");
            Assert.AreEqual(CurrencyConverterPage.ErrorMessageLabel.GetText(), errorMessage, "Error message differs from expected");
        }

        [Test(Description = "AC8")]
        [TestCase("US Dollar", "Euro", "10")]
        public void ConversionCanBeSwapped(string source, string target, string amount)
        {
            CurrencyConverterPage.ConvertCurrencies(amount, source, target);
            CurrencyConverterPage.SwapConversion();

            Assert.IsTrue(CurrencyConverterPage.SelectedSourceCurrency.GetText().Contains(target), "Source was not swapped");
            Assert.IsTrue(CurrencyConverterPage.SelectedTargetCurrency.GetText().Contains(source), "Target was not swapped");

            //Get results
            var rate = CurrencyConverterPage.GetUnitRate();
            var reverseRate = CurrencyConverterPage.GetReverseUnitRate();
            var convertedResult = Math.Round(CurrencyConverterPage.GetConvertedResult(), 2);
            var amountResult = Math.Round(double.Parse(amount), 2);

            //Round values due to inconsistent acceptance criteria
            var mathResult = Math.Round(amountResult * rate, 2);
            Assert.AreEqual(convertedResult, mathResult, "Swapped conversion was incorrect");

            mathResult = Math.Round(amountResult / reverseRate, 2);
            Assert.AreEqual(convertedResult, mathResult, "Swapped conversion was incorrect");
        }

        [Test(Description = "AC9")]
        [TestCase("US Dollar", "Euro", "123")]
        public void ValidateDynamicUrl(string source, string target, string amount)
        {
            var referentUrlText = "https://www.xe.com/currencyconverter/convert/?Amount=123&From=USD&To=EUR";

            CurrencyConverterPage.ConvertCurrencies(amount, source, target);

            var currentUrl = BrowserHelper.GetUrl();

            Assert.AreEqual(referentUrlText, currentUrl, "Url doesn't match to converted values");
        }

        [Test(Description = "AC10")]
        [TestCase("USD", "EUR", "123.22")]
        public void DirectUrlQueryCanBeUsedForConversion(string source, string target, string amount)
        {
            var url = $"https://www.xe.com/currencyconverter/convert/?Amount={amount}&From={source}&To={target}";
            BrowserHelper.GoToUrl(url);

            Assert.IsTrue(CurrencyConverterPage.SelectedSourceCurrency.GetText().Contains(source), "Source differs from expected");
            Assert.IsTrue(CurrencyConverterPage.SelectedTargetCurrency.GetText().Contains(target), "Target differs from expected");

            //Get results of conversion
            var currentAmount = CurrencyConverterPage.GetConvertedAmount();
            var amountResult = Math.Round(double.Parse(amount), 2);
            var rate = CurrencyConverterPage.GetUnitRate();
            var reverseRate = CurrencyConverterPage.GetReverseUnitRate();
            var convertedResult = Math.Round(CurrencyConverterPage.GetConvertedResult(), 2);


            //Round values due to inconsistent acceptance criteria
            Assert.AreEqual(amountResult, currentAmount, "Amount differs from expected");

            var mathResult = Math.Round(amountResult * rate, 2);
            Assert.AreEqual(convertedResult, mathResult, "Converted result differs from expected");

            mathResult = Math.Round(amountResult / reverseRate, 2);
            Assert.AreEqual(convertedResult, mathResult, "Converted result differs from expected");
        }
    }
}