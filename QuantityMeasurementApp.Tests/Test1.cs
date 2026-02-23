using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    }
}