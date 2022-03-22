using System;
using NUnit.Framework;
using static VerificationService.IsoCurrencyValidator;

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable CA1707 // Identifiers should not contain underscores

namespace BookClass.Tests
{
    [TestFixture]
    public class IsoCurrencyValidatorTests
    {
        [TestCase("BYN")]
        [TestCase("EUR")]
        [TestCase("USD")]
        [TestCase("BRL")]
        [TestCase("RUB")]
        [TestCase("UAH")]
        public void IsValid_CurrencyIsValid(string currency)
        {
            Assert.IsTrue(IsValid(currency));
        }

        [TestCase("B-YR")]
        [TestCase("T4_")]
        [TestCase("~*~")]
        [TestCase("F")]
        public void IsValid_InvalidSymbols_CurrencyIsNotValid(string currency)
        {
            Assert.IsFalse(IsValid(currency));
        }

        [TestCase("AAA")]
        [TestCase("ABC")]
        [TestCase("JMA")]
        [TestCase("GRE")]
        [TestCase("HKE")]
        [TestCase("KIR")]
        [TestCase("MOV")]
        [TestCase("ZWY")]
        [TestCase("ZZZ")]
        public void IsValid_CurrencyIsNotValid(string currency)
        {
            Assert.IsFalse(IsValid(currency));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("    ")]
        public void IsValid_CurrencyIsNullOrEmptyOrWhiteSpace_ThrowArgumentException(string currency)
        {
            Assert.Throws<ArgumentException>(() => IsValid(currency), "Currency string cannot be null or empty or whitespace.");
        }
    }
}
