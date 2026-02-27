using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Model;
namespace QuantityMeasurementApp.Tests;
    [TestClass]
    public class FeetTest
    {
        // test same value should return true
        [TestMethod]
        public void testEquality_SameValue()
        {
            Feet f1 = new Feet(1.0);
            Feet f2 = new Feet(1.0);
            bool result = f1.Equals(f2);
            Assert.IsTrue(result, "1.0 ft should be equal to 1.0 ft");
        }

        // test different value should return false
        [TestMethod]
        public void testEquality_DifferentValue()
        {
            Feet f1 = new Feet(1.0);
            Feet f2 = new Feet(2.0);
            bool result = f1.Equals(f2);
            Assert.IsFalse(result, "1.0 ft should not equal 2.0 ft");
        }

        // test null comparison
        [TestMethod]
        public void testEquality_NullComparison()
        {
            Feet f1 = new Feet(1.0);
            bool result = f1.Equals(null);
            Assert.IsFalse(result, "object should not equal null");
        }

        // test same reference
        [TestMethod]
        public void testEquality_SameReference()
        {
            Feet f1 = new Feet(1.0);
            bool result = f1.Equals(f1);
            Assert.IsTrue(result, "object must equal itself");
        }

        // test comparing with different type
        [TestMethod]
        public void testEquality_NonNumericInput()
        {
            Feet f1 = new Feet(1.0);
            object other = "not number";
            bool result = f1.Equals(other);
            Assert.IsFalse(result, "Feet should not equal non Feet object");
        }
    }