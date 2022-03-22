﻿using System;
using NUnit.Framework;
using static VerificationService.IsbnVerifier;

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable CA1707 // Identifiers should not contain underscores

namespace BookClass.Tests
{
    [TestFixture]
    public class IsbnVerifierTests
    {
        [TestCase("978-0-901-69066-1")]
        [TestCase("3-598-21508-8")]
        [TestCase("3-598-21507-X")]
        [TestCase("3598215088")]
        [TestCase("359821507X")]
        [TestCase("3-598-215088")]
        [TestCase("039304002X")]
        public void IsValid_IsbnIsValid(string number)
        {
            Assert.IsTrue(IsValid(number));
        }

        [TestCase("978-0-901-69066-X")]
        [TestCase("----3-5--98-21508-8")]
        [TestCase("3-5-98-21507-X")]
        [TestCase("35A98-2150----88")]
        [TestCase("3-5982yb1507-X")]
        [TestCase("35982yb1507gX")]
        [TestCase("---359821507X")]
        [TestCase("359821507---X")]
        [TestCase("359821507--FX")]
        [TestCase("359821507--XX")]
        [TestCase("359X821507--X")]
        public void IsValid_InvalidSymbols_IsbnIsNotValid(string number)
        {
            Assert.IsFalse(IsValid(number));
        }

        [TestCase("3-598-21508-9")]
        [TestCase("3-598-21507-A")]
        [TestCase("3-598-P1581-X")]
        [TestCase("3-598-2X507-9")]
        [TestCase("359821507")]
        [TestCase("3598215078X")]
        [TestCase("00")]
        [TestCase("3-598-21507")]
        [TestCase("3-598-21515-X")]
        [TestCase("134456729")]
        [TestCase("3132P34035")]
        [TestCase("98245726788")]
        public void IsValid_IsbnIsNotValid(string number)
        {
            Assert.IsFalse(IsValid(number));
        }

        [TestCase(null)]
        public void IsValid_StringIsNullOrEmptyOrWhiteSpace_ThrowArgumentException(string number)
        {
            Assert.Throws<ArgumentNullException>(() => IsValid(number), "Source string cannot be null.");
        }
    }
}
