﻿﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Model;
using System;

namespace QuantityMeasurementAppTests
{
    [TestClass]
    public class Test1
    {
        private const double EPSILON = 1e-6;

        [TestMethod]
        public void testConversion_FeetToInches()
        {
            Quantity q = new Quantity(1.0, Unit.FEET);
            double result = q.ConvertTo(Unit.INCH);

            Assert.AreEqual(12.0, result, EPSILON);
        }

        [TestMethod]
        public void testConversion_InchesToFeet()
        {
            Quantity q = new Quantity(24.0, Unit.INCH);
            double result = q.ConvertTo(Unit.FEET);

            Assert.AreEqual(2.0, result, EPSILON);
        }

        [TestMethod]
        public void testConversion_YardsToInches()
        {
            Quantity q = new Quantity(1.0, Unit.YARD);
            double result = q.ConvertTo(Unit.INCH);

            Assert.AreEqual(36.0, result, EPSILON);
        }

        [TestMethod]
        public void testConversion_InchesToYards()
        {
            Quantity q = new Quantity(72.0, Unit.INCH);
            double result = q.ConvertTo(Unit.YARD);

            Assert.AreEqual(2.0, result, EPSILON);
        }

        [TestMethod]
        public void testConversion_CentimetersToInches()
        {
            Quantity q = new Quantity(2.54, Unit.CENTIMETER);
            double result = q.ConvertTo(Unit.INCH);

            Assert.AreEqual(1.0, result, EPSILON);
        }

        [TestMethod]
        public void testConversion_FeetToYard()
        {
            Quantity q = new Quantity(6.0, Unit.FEET);
            double result = q.ConvertTo(Unit.YARD);

            Assert.AreEqual(2.0, result, EPSILON);
        }

        [TestMethod]
        public void testConversion_RoundTrip_PreservesValue()
        {
            double original = 5.5;

            Quantity q = new Quantity(original, Unit.FEET);
            double toInch = q.ConvertTo(Unit.INCH);

            Quantity back = new Quantity(toInch, Unit.INCH);
            double result = back.ConvertTo(Unit.FEET);

            Assert.AreEqual(original, result, EPSILON);
        }

        [TestMethod]
        public void testConversion_ZeroValue()
        {
            Quantity q = new Quantity(0.0, Unit.FEET);
            double result = q.ConvertTo(Unit.INCH);

            Assert.AreEqual(0.0, result, EPSILON);
        }

        [TestMethod]
        public void testConversion_NegativeValue()
        {
            Quantity q = new Quantity(-1.0, Unit.FEET);
            double result = q.ConvertTo(Unit.INCH);

            Assert.AreEqual(-12.0, result, EPSILON);
        }

        [TestMethod]
        public void testConversion_InvalidUnit_Throws()
        {
            try
            {
                Quantity q = new Quantity(1.0, (Unit)999);
            }
            catch (ArgumentException)
            {
                return;
            }

            Assert.Fail("Expected exception not thrown");
        }

        [TestMethod]
        public void testConversion_NaNOrInfinite_Throws()
        {
            try
            {
                Quantity q = new Quantity(double.NaN, Unit.FEET);
            }
            catch (ArgumentException)
            {
                return;
            }

            Assert.Fail("Expected exception not thrown");
        }

        [TestMethod]
        public void testConversion_PrecisionTolerance()
        {
            Quantity q = new Quantity(1.0, Unit.CENTIMETER);
            double result = q.ConvertTo(Unit.INCH);

            Assert.AreEqual(0.3937008, result, EPSILON);
        }
    }
}