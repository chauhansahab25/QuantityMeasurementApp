
﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp;

namespace QuantityMeasurementAppTests
{
    [TestClass]
    public class QuantityMeasurementAppTests
    {
        [TestMethod]
        public void TestEquality_SameValue()
        {
            var f1 = new QuantityMeasurementApp.Feet(1.0);
            var f2 = new QuantityMeasurementApp.Feet(1.0);

            Assert.IsTrue(f1.Equals(f2),
                "1.0 ft should be equal to 1.0 ft");
        }

        [TestMethod]
        public void TestEquality_DifferentValue()
        {
            var f1 = new QuantityMeasurementApp.Feet(1.0);
            var f2 = new QuantityMeasurementApp.Feet(2.0);

            Assert.IsFalse(f1.Equals(f2),
                "1.0 ft should not equal 2.0 ft");
        }

        [TestMethod]
        public void TestEquality_NullComparison()
        {
            var f1 = new QuantityMeasurementApp.Feet(1.0);

            Assert.IsFalse(f1.Equals(null),
                "Feet value should not equal null");
        }

        [TestMethod]
        public void TestEquality_NonNumericInput()
        {
            var f1 = new QuantityMeasurementApp.Feet(1.0);

            Assert.IsFalse(f1.Equals("1.0"),
                "Feet object should not equal non-numeric input");
        }

        [TestMethod]
        public void TestEquality_SameReference()
        {
            var f1 = new QuantityMeasurementApp.Feet(1.0);

            Assert.IsTrue(f1.Equals(f1),
                "Object should be equal to itself (reflexive)");
        }
        [TestClass]
    public class InchesEqualityTests
    {
        [TestMethod]
        public void TestInchesEquality_SameValue()
        {
            var inch1 = new QuantityMeasurementApp.Inches(1.0);
            var inch2 = new QuantityMeasurementApp.Inches(1.0);

            Assert.IsTrue(inch1.Equals(inch2));
        }

        [TestMethod]
        public void TestInchesEquality_DifferentValue()
        {
            var inch1 = new QuantityMeasurementApp.Inches(1.0);
            var inch2 = new QuantityMeasurementApp.Inches(2.0);

            Assert.IsFalse(inch1.Equals(inch2));
        }

        [TestMethod]
        public void TestInchesEquality_NullComparison()
        {
            var inch = new QuantityMeasurementApp.Inches(1.0);

            Assert.IsFalse(inch.Equals(null));
        }

        [TestMethod]
        public void TestInchesEquality_DifferentClass()
        {
            var inch = new QuantityMeasurementApp.Inches(1.0);

            Assert.IsFalse(inch.Equals("Invalid Type"));
        }

        [TestMethod]
        public void TestInchesEquality_SameReference()
        {
            var inch = new QuantityMeasurementApp.Inches(1.0);

            Assert.IsTrue(inch.Equals(inch));
        }
    }
    }
}
