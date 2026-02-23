using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Model;
using System;

namespace QuantityMeasurementAppTests
{
    [TestClass]
    public class QuantityTests
    {
        [TestMethod]
        public void testEquality_FeetToFeet_SameValue()
        {
            var q1 = new Quantity(1.0, Unit.FEET);
            var q2 = new Quantity(1.0, Unit.FEET);

            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void testEquality_InchToInch_SameValue()
        {
            var q1 = new Quantity(1.0, Unit.INCH);
            var q2 = new Quantity(1.0, Unit.INCH);

            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void testEquality_FeetToInch_EquivalentValue()
        {
            var q1 = new Quantity(1.0, Unit.FEET);
            var q2 = new Quantity(12.0, Unit.INCH);

            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void testEquality_InchToFeet_EquivalentValue()
        {
            var q1 = new Quantity(12.0, Unit.INCH);
            var q2 = new Quantity(1.0, Unit.FEET);

            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void testEquality_FeetToFeet_DifferentValue()
        {
            var q1 = new Quantity(1.0, Unit.FEET);
            var q2 = new Quantity(2.0, Unit.FEET);

            Assert.IsFalse(q1.Equals(q2));
        }

        [TestMethod]
        public void testEquality_InchToInch_DifferentValue()
        {
            var q1 = new Quantity(1.0, Unit.INCH);
            var q2 = new Quantity(2.0, Unit.INCH);

            Assert.IsFalse(q1.Equals(q2));
        }

        [TestMethod]
public void testEquality_InvalidUnit()
{
    try
    {
        new Quantity(1.0, (Unit)999);
        Assert.Fail("Expected ArgumentException was not thrown.");
    }
    catch (ArgumentException)
    {
    }
}

        [TestMethod]
        public void testEquality_SameReference()
        {
            var q = new Quantity(1.0, Unit.FEET);
            Assert.IsTrue(q.Equals(q));
        }

        [TestMethod]
        public void testEquality_NullComparison()
        {
            var q = new Quantity(1.0, Unit.FEET);
            Assert.IsFalse(q.Equals(null));
        }

        [TestMethod]
    public void testEquality_NullUnit()
   {
    try
    {
        new Quantity(1.0, (Unit)999);
        Assert.Fail("Expected ArgumentException was not thrown.");
    }
    catch (ArgumentException)
    {
        
    }
}
    }
}