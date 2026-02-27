using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Model;
namespace QuantityMeasurementApp.Tests;

[TestClass]
public
class InchesTest
{
    // test same value should return true
    [TestMethod]
    public void testEquality_SameValue()
    {
        Inches i1 = new Inches(12.0);
        Inches i2 = new Inches(12.0);
        bool result = i1.Equals(i2);
        Assert.IsTrue(result, "12.0 in should be equal to 12.0 in");
    }

    // test different value should return false
    [TestMethod]
    public void testEquality_DifferentValue()
    {
        Inches i1 = new Inches(12.0);
        Inches i2 = new Inches(24.0);
        bool result = i1.Equals(i2);
        Assert.IsFalse(result, "12.0 in should not equal 24.0 in");
    }

    // test null comparison
    [TestMethod]
    public void testEquality_NullComparison()
    {
        Inches i1 = new Inches(12.0);
        bool result = i1.Equals(null);
        Assert.IsFalse(result, "object should not equal null");
    }

    // test same reference
    [TestMethod]
    public void testEquality_SameReference()
    {
        Inches i1 = new Inches(12.0);
        bool result = i1.Equals(i1);
        Assert.IsTrue(result, "object must equal itself");
    }

    // test comparing with different type
    [TestMethod]
    public void testEquality_NonNumericInput()
    {
        Inches i1 = new Inches(12.0);
        object other = "not number";
        bool result = i1.Equals(other);
        Assert.IsFalse(result, "Inches should not equal non Inches object");
    }
}