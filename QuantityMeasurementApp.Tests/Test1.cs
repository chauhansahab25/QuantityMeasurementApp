﻿﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Model;
using System;

namespace QuantityMeasurementApp.Tests
{
   [TestClass]
    public class QuantityLengthTest
    {
        [TestMethod]
        public void TestEquality_FeetToFeet_SameValue()
        {
            Quantity q1 = new Quantity(1.0, Unit.FEET);
            Quantity q2 = new Quantity(1.0, Unit.FEET);
            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void TestEquality_InchToInch_SameValue()
        {
            Quantity q1 = new Quantity(1.0, Unit.INCH);
            Quantity q2 = new Quantity(1.0, Unit.INCH);
            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void TestEquality_FeetToInch_EquivalentValue()
        {
            Quantity q1 = new Quantity(1.0, Unit.FEET);
            Quantity q2 = new Quantity(12.0,Unit.INCH);
            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void TestEquality_DifferentValue()
        {
            Quantity q1 = new Quantity(1.0, Unit.FEET);
            Quantity q2 = new Quantity(2.0, Unit.FEET);
            Assert.IsFalse(q1.Equals(q2));
        }

        [TestMethod]
        public void TestEquality_NullComparison()
        {
            Quantity q1 = new Quantity(1.0, Unit.FEET);
            Assert.IsFalse(q1.Equals(null));
        }

        [TestMethod]
        public void TestEquality_SameReference()
        {
            Quantity q1 = new Quantity(1.0, Unit.FEET);
            Assert.IsTrue(q1.Equals(q1));
        }

        [TestMethod]
        public void TestEquality_YardToFeet_EquivalentValue()
        {
            Quantity q1 = new Quantity(1.0, Unit.YARD);
            Quantity q2 = new Quantity(3.0, Unit.FEET);

            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void TestEquality_YardToInch_EquivalentValue()
        {
            Quantity q1 = new Quantity(1.0, Unit.YARD);
            Quantity q2 = new Quantity(36.0, Unit.INCH);

            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void TestEquality_CentimeterToInch_EquivalentValue()
        {
            Quantity q1 = new Quantity(1.0, Unit.CENTIMETER);
            Quantity q2 = new Quantity(0.393701, Unit.INCH);

            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void TestEquality_YardToYard_DifferentValue()
        {
            Quantity q1 = new Quantity(1.0, Unit.YARD);
            Quantity q2 = new Quantity(2.0, Unit.YARD);

            Assert.IsFalse(q1.Equals(q2));
        }

        [TestMethod]
        public void TestConversion_RoundTrip()
        {
            double original = 5.0;

            double toYard = Quantity.Convert(original, Unit.FEET, Unit.YARD);
            double backToFeet = Quantity.Convert(toYard, Unit.YARD, Unit.FEET);

            Assert.AreEqual(original, backToFeet, 0.000001);
        }

        [TestMethod]
        public void TestConversion_NegativeValue()
        {
            double result = Quantity.Convert(-1.0, Unit.FEET, Unit.INCH);
            Assert.AreEqual(-12.0, result, 0.000001);
        }

        //Testing conversion of a value from one unit to the same unit should return the original value
        [TestMethod]
        public void TestConversion_ZeroValue()
        {
            double result = Quantity.Convert(0.0, Unit.FEET, Unit.INCH);
            Assert.AreEqual(0.0, result, 0.000001);
        }

        //Testing conversion of a value from one unit to the same unit should return the original value
        [TestMethod]
        public void TestConversion_CentimeterToInch()
        {
            double result = Quantity.Convert(2.54, Unit.CENTIMETER, Unit.INCH);
            Assert.AreEqual(1.0, result, 0.0001);
        }
        //Testing conversion of a value from one unit to the same unit should return the original value
        [TestMethod]
        public void TestConversion_YardsToFeet()
        {
            double result = Quantity.Convert(3.0, Unit.YARD, Unit.FEET);
            Assert.AreEqual(9.0, result, 0.000001);
        }
        //Testing conversion of a value from one unit to the same unit should return the original value
        [TestMethod]
        public void TestConversion_InchesToFeet()
        {
            double result = Quantity.Convert(24.0, Unit.INCH, Unit.FEET);
            Assert.AreEqual(2.0, result, 0.000001);
        }
        //Testing conversion of a value from one unit to the same unit should return the original value
        [TestMethod]
        public void TestConversion_FeetToInches()
        {
            double result = Quantity.Convert(1.0, Unit.FEET, Unit.INCH);
            Assert.AreEqual(12.0, result, 0.000001);
        }

        //Testing addition of two lengths where one operand is null
        [TestMethod]
        public void TestAddition_NullSecondOperand()
        {
            Quantity q1 = new Quantity(1.0, Unit.FEET);
            try
            {
                q1.Add(null);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException)
            {
            
            }
        }

    
        [TestMethod]
        public void TestAddition_NegativeValue()
        {
            Quantity q1 = new Quantity(5.0, Unit.FEET);
            Quantity q2 = new Quantity(-2.0, Unit.FEET);

            Quantity result = q1.Add(q2);

            Assert.IsTrue(result.Equals(new Quantity(3.0, Unit.FEET)));
        }
        //Testing addition of zero to a length
        [TestMethod]
        public void TestAddition_WithZero()
        {
            Quantity q1 = new Quantity(5.0, Unit.FEET);
            Quantity q2 = new Quantity(0.0, Unit.INCH);

            Quantity result = q1.Add(q2);

            Assert.IsTrue(result.Equals(new Quantity(5.0, Unit.FEET)));
        }
        [TestMethod]
        public void TestAddition_CrossUnit_InchPlusFeet()
        {
            Quantity q1 = new Quantity(12.0, Unit.INCH);
            Quantity q2 = new Quantity(1.0, Unit.FEET);

            Quantity result = q1.Add(q2);

            Assert.IsTrue(result.Equals(new Quantity(24.0, Unit.INCH)));
        }
        [TestMethod]
        public void TestAddition_CrossUnit_FeetPlusInch()
        {
            Quantity q1 = new Quantity(1.0, Unit.FEET);
            Quantity q2 = new Quantity(12.0, Unit.INCH);

            Quantity result = q1.Add(q2);

            Assert.IsTrue(result.Equals(new Quantity(2.0, Unit.FEET)));
        }
        [TestMethod]
        public void TestAddition_SameUnit_FeetPlusFeet()
        {
            Quantity q1 = new Quantity(1.0, Unit.FEET);
            Quantity q2 = new Quantity(2.0, Unit.FEET);

            Quantity result = q1.Add(q2);

            Assert.IsTrue(result.Equals(new Quantity(3.0, Unit.FEET)));
        }


    }
}