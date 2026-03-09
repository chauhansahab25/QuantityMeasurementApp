using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Model;
using System;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityVolumeTest
    {
        [TestMethod]
        public void TestEquality_LitreToLitre_SameValue()
        {
            Quantity<VolumeUnit> v1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> v2 = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            Assert.IsTrue(v1.Equals(v2));
        }

        [TestMethod]
        public void TestEquality_MillilitreToMillilitre_SameValue()
        {
            Quantity<VolumeUnit> v1 = new Quantity<VolumeUnit>(1000.0, VolumeUnit.MILLILITRE);
            Quantity<VolumeUnit> v2 = new Quantity<VolumeUnit>(1000.0, VolumeUnit.MILLILITRE);
            Assert.IsTrue(v1.Equals(v2));
        }

        [TestMethod]
        public void TestEquality_GallonToGallon_SameValue()
        {
            Quantity<VolumeUnit> v1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.GALLON);
            Quantity<VolumeUnit> v2 = new Quantity<VolumeUnit>(1.0, VolumeUnit.GALLON);
            Assert.IsTrue(v1.Equals(v2));
        }

        [TestMethod]
        public void TestEquality_LitreToMillilitre_EquivalentValue()
        {
            Quantity<VolumeUnit> v1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> v2 = new Quantity<VolumeUnit>(1000.0, VolumeUnit.MILLILITRE);
            Assert.IsTrue(v1.Equals(v2));
        }

        [TestMethod]
        public void TestEquality_LitreToGallon_EquivalentValue()
        {
            Quantity<VolumeUnit> v1 = new Quantity<VolumeUnit>(3.78541, VolumeUnit.LITRE);
            Quantity<VolumeUnit> v2 = new Quantity<VolumeUnit>(1.0, VolumeUnit.GALLON);
            Assert.IsTrue(v1.Equals(v2));
        }

        [TestMethod]
        public void TestEquality_GallonToLitre_EquivalentValue()
        {
            Quantity<VolumeUnit> v1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.GALLON);
            Quantity<VolumeUnit> v2 = new Quantity<VolumeUnit>(3.78541, VolumeUnit.LITRE);
            Assert.IsTrue(v1.Equals(v2));
        }

        [TestMethod]
        public void TestEquality_DifferentValue()
        {
            Quantity<VolumeUnit> v1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> v2 = new Quantity<VolumeUnit>(2.0, VolumeUnit.LITRE);
            Assert.IsFalse(v1.Equals(v2));
        }

        [TestMethod]
        public void TestEquality_NullComparison()
        {
            Quantity<VolumeUnit> v1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            Assert.IsFalse(v1.Equals(null));
        }

        [TestMethod]
        public void TestEquality_SameReference()
        {
            Quantity<VolumeUnit> v1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            Assert.IsTrue(v1.Equals(v1));
        }

        [TestMethod]
        public void TestEquality_VolumeVsLength_Incompatible()
        {
            Quantity<VolumeUnit> volume = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            Quantity length = new Quantity(1.0, Unit.FEET);
            Assert.IsFalse(volume.Equals(length));
        }

        [TestMethod]
        public void TestEquality_VolumeVsWeight_Incompatible()
        {
            Quantity<VolumeUnit> volume = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            QuantityWeight weight = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            Assert.IsFalse(volume.Equals(weight));
        }

        [TestMethod]
        public void TestConversion_LitreToMillilitre()
        {
            double result = Quantity<VolumeUnit>.Convert(1.0, VolumeUnit.LITRE, VolumeUnit.MILLILITRE);
            Assert.AreEqual(1000.0, result, 0.000001);
        }

        [TestMethod]
        public void TestConversion_MillilitreToLitre()
        {
            double result = Quantity<VolumeUnit>.Convert(1000.0, VolumeUnit.MILLILITRE, VolumeUnit.LITRE);
            Assert.AreEqual(1.0, result, 0.000001);
        }

        [TestMethod]
        public void TestConversion_GallonToLitre()
        {
            double result = Quantity<VolumeUnit>.Convert(1.0, VolumeUnit.GALLON, VolumeUnit.LITRE);
            Assert.AreEqual(3.78541, result, 0.001);
        }

        [TestMethod]
        public void TestConversion_LitreToGallon()
        {
            double result = Quantity<VolumeUnit>.Convert(3.78541, VolumeUnit.LITRE, VolumeUnit.GALLON);
            Assert.AreEqual(1.0, result, 0.001);
        }

        [TestMethod]
        public void TestConversion_RoundTrip()
        {
            double original = 5.0;
            double toGallon = Quantity<VolumeUnit>.Convert(original, VolumeUnit.LITRE, VolumeUnit.GALLON);
            double backToLitre = Quantity<VolumeUnit>.Convert(toGallon, VolumeUnit.GALLON, VolumeUnit.LITRE);
            Assert.AreEqual(original, backToLitre, 0.000001);
        }

        [TestMethod]
        public void TestConversion_ZeroValue()
        {
            double result = Quantity<VolumeUnit>.Convert(0.0, VolumeUnit.LITRE, VolumeUnit.MILLILITRE);
            Assert.AreEqual(0.0, result, 0.000001);
        }

        [TestMethod]
        public void TestConversion_NegativeValue()
        {
            double result = Quantity<VolumeUnit>.Convert(-1.0, VolumeUnit.LITRE, VolumeUnit.MILLILITRE);
            Assert.AreEqual(-1000.0, result, 0.000001);
        }

        [TestMethod]
        public void TestAddition_SameUnit_LitrePlusLitre()
        {
            Quantity<VolumeUnit> v1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> v2 = new Quantity<VolumeUnit>(2.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> result = v1.Add(v2);
            Assert.IsTrue(result.Equals(new Quantity<VolumeUnit>(3.0, VolumeUnit.LITRE)));
        }

        [TestMethod]
        public void TestAddition_CrossUnit_LitrePlusMillilitre()
        {
            Quantity<VolumeUnit> v1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> v2 = new Quantity<VolumeUnit>(1000.0, VolumeUnit.MILLILITRE);
            Quantity<VolumeUnit> result = v1.Add(v2);
            Assert.IsTrue(result.Equals(new Quantity<VolumeUnit>(2.0, VolumeUnit.LITRE)));
        }

        [TestMethod]
        public void TestAddition_CrossUnit_MillilitrePlusLitre()
        {
            Quantity<VolumeUnit> v1 = new Quantity<VolumeUnit>(1000.0, VolumeUnit.MILLILITRE);
            Quantity<VolumeUnit> v2 = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> result = v1.Add(v2);
            Assert.IsTrue(result.Equals(new Quantity<VolumeUnit>(2000.0, VolumeUnit.MILLILITRE)));
        }

        [TestMethod]
        public void TestAddition_CrossUnit_GallonPlusLitre()
        {
            Quantity<VolumeUnit> v1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.GALLON);
            Quantity<VolumeUnit> v2 = new Quantity<VolumeUnit>(3.78541, VolumeUnit.LITRE);
            Quantity<VolumeUnit> result = v1.Add(v2);
            Assert.IsTrue(result.Equals(new Quantity<VolumeUnit>(2.0, VolumeUnit.GALLON)));
        }

        [TestMethod]
        public void TestAddition_WithZero()
        {
            Quantity<VolumeUnit> v1 = new Quantity<VolumeUnit>(5.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> v2 = new Quantity<VolumeUnit>(0.0, VolumeUnit.MILLILITRE);
            Quantity<VolumeUnit> result = v1.Add(v2);
            Assert.IsTrue(result.Equals(new Quantity<VolumeUnit>(5.0, VolumeUnit.LITRE)));
        }

        [TestMethod]
        public void TestAddition_NegativeValue()
        {
            Quantity<VolumeUnit> v1 = new Quantity<VolumeUnit>(5.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> v2 = new Quantity<VolumeUnit>(-2.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> result = v1.Add(v2);
            Assert.IsTrue(result.Equals(new Quantity<VolumeUnit>(3.0, VolumeUnit.LITRE)));
        }

        [TestMethod]
        public void TestAddition_NullSecondOperand()
        {
            Quantity<VolumeUnit> v1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            try
            {
                v1.Add(null);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public void TestSubtraction_SameUnit_LitreMinusLitre()
        {
            Quantity<VolumeUnit> v1 = new Quantity<VolumeUnit>(10.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> v2 = new Quantity<VolumeUnit>(3.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> result = v1.Subtract(v2);
            Assert.IsTrue(result.Equals(new Quantity<VolumeUnit>(7.0, VolumeUnit.LITRE)));
        }

        [TestMethod]
        public void TestSubtraction_CrossUnit_LitreMinusMillilitre()
        {
            Quantity<VolumeUnit> v1 = new Quantity<VolumeUnit>(5.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> v2 = new Quantity<VolumeUnit>(500.0, VolumeUnit.MILLILITRE);
            Quantity<VolumeUnit> result = v1.Subtract(v2);
            Assert.IsTrue(result.Equals(new Quantity<VolumeUnit>(4.5, VolumeUnit.LITRE)));
        }

        [TestMethod]
        public void TestDivision_SameUnit_LitreDividedByLitre()
        {
            Quantity<VolumeUnit> v1 = new Quantity<VolumeUnit>(10.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> v2 = new Quantity<VolumeUnit>(5.0, VolumeUnit.LITRE);
            double result = v1.Divide(v2);
            Assert.AreEqual(2.0, result, 0.000001);
        }

        [TestMethod]
        public void TestDivision_CrossUnit_MillilitreDividedByLitre()
        {
            Quantity<VolumeUnit> v1 = new Quantity<VolumeUnit>(1000.0, VolumeUnit.MILLILITRE);
            Quantity<VolumeUnit> v2 = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            double result = v1.Divide(v2);
            Assert.AreEqual(1.0, result, 0.000001);
        }
    }
}
