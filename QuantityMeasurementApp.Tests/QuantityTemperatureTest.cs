using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Model;
using QuantityMeasurementModelLayer.Enums;
using QuantityMeasurementModelLayer.Exceptions;
using System;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityTemperatureTest
    {
        [TestMethod]
        public void TestEquality_CelsiusToCelsius_SameValue()
        {
            Quantity<TemperatureUnit> t1 = new Quantity<TemperatureUnit>(0.0, TemperatureUnit.CELSIUS);
            Quantity<TemperatureUnit> t2 = new Quantity<TemperatureUnit>(0.0, TemperatureUnit.CELSIUS);
            Assert.IsTrue(t1.Equals(t2));
        }

        [TestMethod]
        public void TestEquality_FahrenheitToFahrenheit_SameValue()
        {
            Quantity<TemperatureUnit> t1 = new Quantity<TemperatureUnit>(32.0, TemperatureUnit.FAHRENHEIT);
            Quantity<TemperatureUnit> t2 = new Quantity<TemperatureUnit>(32.0, TemperatureUnit.FAHRENHEIT);
            Assert.IsTrue(t1.Equals(t2));
        }

        [TestMethod]
        public void TestEquality_CelsiusToFahrenheit_0Celsius32Fahrenheit()
        {
            Quantity<TemperatureUnit> t1 = new Quantity<TemperatureUnit>(0.0, TemperatureUnit.CELSIUS);
            Quantity<TemperatureUnit> t2 = new Quantity<TemperatureUnit>(32.0, TemperatureUnit.FAHRENHEIT);
            Assert.IsTrue(t1.Equals(t2));
        }

        [TestMethod]
        public void TestEquality_CelsiusToFahrenheit_100Celsius212Fahrenheit()
        {
            Quantity<TemperatureUnit> t1 = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.CELSIUS);
            Quantity<TemperatureUnit> t2 = new Quantity<TemperatureUnit>(212.0, TemperatureUnit.FAHRENHEIT);
            Assert.IsTrue(t1.Equals(t2));
        }

        [TestMethod]
        public void TestEquality_CelsiusToFahrenheit_Negative40Equal()
        {
            Quantity<TemperatureUnit> t1 = new Quantity<TemperatureUnit>(-40.0, TemperatureUnit.CELSIUS);
            Quantity<TemperatureUnit> t2 = new Quantity<TemperatureUnit>(-40.0, TemperatureUnit.FAHRENHEIT);
            Assert.IsTrue(t1.Equals(t2));
        }

        [TestMethod]
        public void TestEquality_DifferentValue()
        {
            Quantity<TemperatureUnit> t1 = new Quantity<TemperatureUnit>(0.0, TemperatureUnit.CELSIUS);
            Quantity<TemperatureUnit> t2 = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.CELSIUS);
            Assert.IsFalse(t1.Equals(t2));
        }

        [TestMethod]
        public void TestEquality_NullComparison()
        {
            Quantity<TemperatureUnit> t1 = new Quantity<TemperatureUnit>(0.0, TemperatureUnit.CELSIUS);
            Assert.IsFalse(t1.Equals(null));
        }

        [TestMethod]
        public void TestEquality_SameReference()
        {
            Quantity<TemperatureUnit> t1 = new Quantity<TemperatureUnit>(25.0, TemperatureUnit.CELSIUS);
            Assert.IsTrue(t1.Equals(t1));
        }

        [TestMethod]
        public void TestEquality_TemperatureVsLength_Incompatible()
        {
            Quantity<TemperatureUnit> temp = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.CELSIUS);
            Quantity<LengthUnit> length = new Quantity<LengthUnit>(100.0, LengthUnit.FEET);
            Assert.IsFalse(temp.Equals(length));
        }

        [TestMethod]
        public void TestEquality_TemperatureVsWeight_Incompatible()
        {
            Quantity<TemperatureUnit> temp = new Quantity<TemperatureUnit>(50.0, TemperatureUnit.CELSIUS);
            QuantityWeight weight = new QuantityWeight(50.0, WeightUnit.KILOGRAM);
            Assert.IsFalse(temp.Equals(weight));
        }

        [TestMethod]
        public void TestConversion_CelsiusToFahrenheit()
        {
            double result = Quantity<TemperatureUnit>.Convert(100.0, TemperatureUnit.CELSIUS, TemperatureUnit.FAHRENHEIT);
            Assert.AreEqual(212.0, result, 0.001);
        }

        [TestMethod]
        public void TestConversion_FahrenheitToCelsius()
        {
            double result = Quantity<TemperatureUnit>.Convert(32.0, TemperatureUnit.FAHRENHEIT, TemperatureUnit.CELSIUS);
            Assert.AreEqual(0.0, result, 0.001);
        }

        [TestMethod]
        public void TestConversion_RoundTrip()
        {
            double original = 25.0;
            double toFahrenheit = Quantity<TemperatureUnit>.Convert(original, TemperatureUnit.CELSIUS, TemperatureUnit.FAHRENHEIT);
            double backToCelsius = Quantity<TemperatureUnit>.Convert(toFahrenheit, TemperatureUnit.FAHRENHEIT, TemperatureUnit.CELSIUS);
            Assert.AreEqual(original, backToCelsius, 0.001);
        }

        [TestMethod]
        public void TestConversion_NegativeValue()
        {
            double result = Quantity<TemperatureUnit>.Convert(-40.0, TemperatureUnit.CELSIUS, TemperatureUnit.FAHRENHEIT);
            Assert.AreEqual(-40.0, result, 0.001);
        }

        [TestMethod]
        public void TestUnsupportedOperation_Addition()
        {
            Quantity<TemperatureUnit> t1 = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.CELSIUS);
            Quantity<TemperatureUnit> t2 = new Quantity<TemperatureUnit>(50.0, TemperatureUnit.CELSIUS);
            try
            {
                t1.Add(t2, TemperatureUnit.CELSIUS);
                Assert.Fail("Expected UnsupportedOperationException was not thrown.");
            }
            catch (UnsupportedOperationException ex)
            {
                Assert.IsTrue(ex.Message.Contains("Temperature does not support"));
            }
        }

        [TestMethod]
        public void TestUnsupportedOperation_Subtraction()
        {
            Quantity<TemperatureUnit> t1 = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.CELSIUS);
            Quantity<TemperatureUnit> t2 = new Quantity<TemperatureUnit>(50.0, TemperatureUnit.CELSIUS);
            try
            {
                t1.Subtract(t2);
                Assert.Fail("Expected UnsupportedOperationException was not thrown.");
            }
            catch (UnsupportedOperationException ex)
            {
                Assert.IsTrue(ex.Message.Contains("Temperature does not support"));
            }
        }

        [TestMethod]
        public void TestUnsupportedOperation_Division()
        {
            Quantity<TemperatureUnit> t1 = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.CELSIUS);
            Quantity<TemperatureUnit> t2 = new Quantity<TemperatureUnit>(50.0, TemperatureUnit.CELSIUS);
            try
            {
                t1.Divide(t2);
                Assert.Fail("Expected UnsupportedOperationException was not thrown.");
            }
            catch (UnsupportedOperationException ex)
            {
                Assert.IsTrue(ex.Message.Contains("Temperature does not support"));
            }
        }

        [TestMethod]
        public void TestSupportsArithmetic_ReturnsFalse()
        {
            bool supports = TemperatureUnit.CELSIUS.SupportsArithmetic();
            Assert.IsFalse(supports);
        }
    }
}