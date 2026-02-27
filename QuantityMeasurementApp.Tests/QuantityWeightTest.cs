using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Model;
using System;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityWeightTest
    {
        [TestMethod]
        public void TestEquality_KilogramToKilogram_SameValue()
        {
            QuantityWeight w1 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            QuantityWeight w2 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            Assert.IsTrue(w1.Equals(w2));
        }

        [TestMethod]
        public void TestEquality_GramToGram_SameValue()
        {
            QuantityWeight w1 = new QuantityWeight(1000.0, WeightUnit.GRAM);
            QuantityWeight w2 = new QuantityWeight(1000.0, WeightUnit.GRAM);
            Assert.IsTrue(w1.Equals(w2));
        }

        [TestMethod]
        public void TestEquality_PoundToPound_SameValue()
        {
            QuantityWeight w1 = new QuantityWeight(1.0, WeightUnit.POUND);
            QuantityWeight w2 = new QuantityWeight(1.0, WeightUnit.POUND);
            Assert.IsTrue(w1.Equals(w2));
        }

        [TestMethod]
        public void TestEquality_KilogramToGram_EquivalentValue()
        {
            QuantityWeight w1 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            QuantityWeight w2 = new QuantityWeight(1000.0, WeightUnit.GRAM);
            Assert.IsTrue(w1.Equals(w2));
        }

        [TestMethod]
        public void TestEquality_KilogramToPound_EquivalentValue()
        {
            QuantityWeight w1 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            QuantityWeight w2 = new QuantityWeight(1.0 / 0.453592, WeightUnit.POUND);
            Assert.IsTrue(w1.Equals(w2));
        }

        [TestMethod]
        public void TestEquality_GramToPound_EquivalentValue()
        {
            QuantityWeight w1 = new QuantityWeight(453.592, WeightUnit.GRAM);
            QuantityWeight w2 = new QuantityWeight(1.0, WeightUnit.POUND);
            Assert.IsTrue(w1.Equals(w2));
        }

        [TestMethod]
        public void TestEquality_DifferentValue()
        {
            QuantityWeight w1 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            QuantityWeight w2 = new QuantityWeight(2.0, WeightUnit.KILOGRAM);
            Assert.IsFalse(w1.Equals(w2));
        }

        [TestMethod]
        public void TestEquality_NullComparison()
        {
            QuantityWeight w1 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            Assert.IsFalse(w1.Equals(null));
        }

        [TestMethod]
        public void TestEquality_SameReference()
        {
            QuantityWeight w1 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            Assert.IsTrue(w1.Equals(w1));
        }

        [TestMethod]
        public void TestConversion_KilogramToGram()
        {
            double result = QuantityWeight.Convert(1.0, WeightUnit.KILOGRAM, WeightUnit.GRAM);
            Assert.AreEqual(1000.0, result, 0.000001);
        }

        [TestMethod]
        public void TestConversion_GramToKilogram()
        {
            double result = QuantityWeight.Convert(1000.0, WeightUnit.GRAM, WeightUnit.KILOGRAM);
            Assert.AreEqual(1.0, result, 0.000001);
        }

        [TestMethod]
        public void TestConversion_KilogramToPound()
        {
            double result = QuantityWeight.Convert(1.0, WeightUnit.KILOGRAM, WeightUnit.POUND);
            Assert.AreEqual(2.204622, result, 0.001);
        }

        [TestMethod]
        public void TestConversion_PoundToKilogram()
        {
            double result = QuantityWeight.Convert(2.20462, WeightUnit.POUND, WeightUnit.KILOGRAM);
            Assert.AreEqual(1.0, result, 0.001);
        }

        [TestMethod]
        public void TestConversion_RoundTrip()
        {
            double original = 5.0;
            double toPound = QuantityWeight.Convert(original, WeightUnit.KILOGRAM, WeightUnit.POUND);
            double backToKg = QuantityWeight.Convert(toPound, WeightUnit.POUND, WeightUnit.KILOGRAM);
            Assert.AreEqual(original, backToKg, 0.000001);
        }

        [TestMethod]
        public void TestConversion_ZeroValue()
        {
            double result = QuantityWeight.Convert(0.0, WeightUnit.KILOGRAM, WeightUnit.GRAM);
            Assert.AreEqual(0.0, result, 0.000001);
        }

        [TestMethod]
        public void TestConversion_NegativeValue()
        {
            double result = QuantityWeight.Convert(-1.0, WeightUnit.KILOGRAM, WeightUnit.GRAM);
            Assert.AreEqual(-1000.0, result, 0.000001);
        }

        [TestMethod]
        public void TestAddition_SameUnit_KilogramPlusKilogram()
        {
            QuantityWeight w1 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            QuantityWeight w2 = new QuantityWeight(2.0, WeightUnit.KILOGRAM);
            QuantityWeight result = w1.Add(w2);
            Assert.IsTrue(result.Equals(new QuantityWeight(3.0, WeightUnit.KILOGRAM)));
        }

        [TestMethod]
        public void TestAddition_CrossUnit_KilogramPlusGram()
        {
            QuantityWeight w1 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            QuantityWeight w2 = new QuantityWeight(500.0, WeightUnit.GRAM);
            QuantityWeight result = w1.Add(w2);
            Assert.IsTrue(result.Equals(new QuantityWeight(1.5, WeightUnit.KILOGRAM)));
        }

        [TestMethod]
        public void TestAddition_CrossUnit_GramPlusKilogram()
        {
            QuantityWeight w1 = new QuantityWeight(500.0, WeightUnit.GRAM);
            QuantityWeight w2 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            QuantityWeight result = w1.Add(w2);
            Assert.IsTrue(result.Equals(new QuantityWeight(1500.0, WeightUnit.GRAM)));
        }

        [TestMethod]
        public void TestAddition_WithZero()
        {
            QuantityWeight w1 = new QuantityWeight(5.0, WeightUnit.KILOGRAM);
            QuantityWeight w2 = new QuantityWeight(0.0, WeightUnit.GRAM);
            QuantityWeight result = w1.Add(w2);
            Assert.IsTrue(result.Equals(new QuantityWeight(5.0, WeightUnit.KILOGRAM)));
        }

        [TestMethod]
        public void TestAddition_NegativeValue()
        {
            QuantityWeight w1 = new QuantityWeight(5.0, WeightUnit.KILOGRAM);
            QuantityWeight w2 = new QuantityWeight(-2.0, WeightUnit.KILOGRAM);
            QuantityWeight result = w1.Add(w2);
            Assert.IsTrue(result.Equals(new QuantityWeight(3.0, WeightUnit.KILOGRAM)));
        }

        [TestMethod]
        public void TestAddition_NullSecondOperand()
        {
            QuantityWeight w1 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            try
            {
                w1.Add(null);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public void TestIncompatibility_LengthVsWeight()
        {
            Quantity length = new Quantity(1.0, Unit.FEET);
            QuantityWeight weight = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            Assert.IsFalse(length.Equals(weight));
        }
    }
}
