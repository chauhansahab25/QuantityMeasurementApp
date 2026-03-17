using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Model;
using QuantityMeasurementModelLayer.Exceptions;
using QuantityMeasurementModelLayer.Enums;
using QuantityMeasurementModelLayer.Models;
using QuantityMeasurementRepositoryLayer.Repositories;
using QuantityMeasurementRepositoryLayer.Interfaces;
using QuantityMeasurementModelLayer.DTO;
using QuantityMeasurementModelLayer.Entities;
using Microsoft.Extensions.Configuration;


namespace QuantityMeasurementApp.Tests;
// Unit tests for the QuantityLength class
[TestClass]
public class QuantityLengthTest
{
    //Testing equality of two lengths in the same unit and same value should return true
    [TestMethod]
    public void TestEquality_FEETToFEET_SameValue()
    {
        QuantityLength firstValue = new QuantityLength(1.0, LengthUnit.FEET);
        QuantityLength secondValue = new QuantityLength(1.0, LengthUnit.FEET);
        Assert.IsTrue(firstValue.Equals(secondValue));
    }

    //Testing equality of two lengths in the same unit but different values should return false
    [TestMethod]
    public void TestEquality_INCHESToINCHES_SameValue()
    {
        QuantityLength firstValue = new QuantityLength(1.0, LengthUnit.INCHES);
        QuantityLength secondValue = new QuantityLength(1.0, LengthUnit.INCHES);
        Assert.IsTrue(firstValue.Equals(secondValue));
    }

    //Testing equality of two lengths in different units but equivalent values should return true
    [TestMethod]
    public void TestEquality_FEETToINCHES_EquivalentValue()
    {
        QuantityLength firstValue = new QuantityLength(1.0, LengthUnit.FEET);
        QuantityLength secondValue = new QuantityLength(12.0, LengthUnit.INCHES);
        Assert.IsTrue(firstValue.Equals(secondValue));
    }

    //Testing equality of two lengths in the same unit but different values should return false
    [TestMethod]
    public void TestEquality_DifferentValue()
    {
        QuantityLength firstValue = new QuantityLength(1.0, LengthUnit.FEET);
        QuantityLength secondValue = new QuantityLength(2.0, LengthUnit.FEET);
        Assert.IsFalse(firstValue.Equals(secondValue));
    }

    //Testing equality of a QuantityLength object with null should return false
    [TestMethod]
    public void TestEquality_NullComparison()
    {
        QuantityLength firstValue = new QuantityLength(1.0, LengthUnit.FEET);
        Assert.IsFalse(firstValue.Equals(null));
    }

    //Testing equality of the same reference should return true
    [TestMethod]
    public void TestEquality_SameReference()
    {
        QuantityLength firstValue = new QuantityLength(1.0, LengthUnit.FEET);
        Assert.IsTrue(firstValue.Equals(firstValue));
    }

    //Testing equality of two lengths in different units but equivalent values should return true
    [TestMethod]
    public void TestEquality_YARDSToFEET_EquivalentValue()
    {
        QuantityLength firstValue = new QuantityLength(1.0, LengthUnit.YARDS);
        QuantityLength secondValue = new QuantityLength(3.0, LengthUnit.FEET);

        Assert.IsTrue(firstValue.Equals(secondValue));
    }

    //Testing equality of two lengths in different units but equivalent values should return true
    [TestMethod]
    public void TestEquality_YARDSToINCHES_EquivalentValue()
    {
        QuantityLength firstValue = new QuantityLength(1.0, LengthUnit.YARDS);
        QuantityLength secondValue = new QuantityLength(36.0, LengthUnit.INCHES);

        Assert.IsTrue(firstValue.Equals(secondValue));
    }

    //Testing equality of two lengths in different units but equivalent values should return true
    [TestMethod]
    public void TestEquality_CENTIMETERSToINCHES_EquivalentValue()
    {
        QuantityLength firstValue = new QuantityLength(1.0, LengthUnit.CENTIMETERS);
        QuantityLength secondValue = new QuantityLength(0.393701, LengthUnit.INCHES);

        Assert.IsTrue(firstValue.Equals(secondValue));
    }

    //Testing equality of two lengths in the same unit but different values should return false
    [TestMethod]
    public void TestEquality_YARDSToYARDS_DifferentValue()
    {
        QuantityLength firstValue = new QuantityLength(1.0, LengthUnit.YARDS);
        QuantityLength secondValue = new QuantityLength(2.0, LengthUnit.YARDS);

        Assert.IsFalse(firstValue.Equals(secondValue));
    }

    //Testing conversion of a value from one unit to the same unit should return the original value
    [TestMethod]
    public void TestConversion_RoundTrip()
    {
        double original = 5.0;

        double toYARDS = QuantityLength.Convert(original, LengthUnit.FEET, LengthUnit.YARDS);
        double backToFEET = QuantityLength.Convert(toYARDS, LengthUnit.YARDS, LengthUnit.FEET);

        Assert.AreEqual(original, backToFEET, 0.000001);
    }

    //Testing conversion of a value from one unit to the same unit should return the original value
    [TestMethod]
    public void TestConversion_NegativeValue()
    {
        double result = QuantityLength.Convert(-1.0, LengthUnit.FEET, LengthUnit.INCHES);
        Assert.AreEqual(-12.0, result, 0.000001);
    }

    //Testing conversion of a value from one unit to the same unit should return the original value
    [TestMethod]
    public void TestConversion_ZeroValue()
    {
        double result = QuantityLength.Convert(0.0, LengthUnit.FEET, LengthUnit.INCHES);
        Assert.AreEqual(0.0, result, 0.000001);
    }

    //Testing conversion of a value from one unit to the same unit should return the original value
    [TestMethod]
    public void TestConversion_CENTIMETERSToINCHES()
    {
        double result = QuantityLength.Convert(2.54, LengthUnit.CENTIMETERS, LengthUnit.INCHES);
        Assert.AreEqual(1.0, result, 0.0001);
    }
    //Testing conversion of a value from one unit to the same unit should return the original value
    [TestMethod]
    public void TestConversion_YARDSsToFEET()
    {
        double result = QuantityLength.Convert(3.0, LengthUnit.YARDS, LengthUnit.FEET);
        Assert.AreEqual(9.0, result, 0.000001);
    }
    //Testing conversion of a value from one unit to the same unit should return the original value
    [TestMethod]
    public void TestConversion_INCHESesToFEET()
    {
        double result = QuantityLength.Convert(24.0, LengthUnit.INCHES, LengthUnit.FEET);
        Assert.AreEqual(2.0, result, 0.000001);
    }
    //Testing conversion of a value from one unit to the same unit should return the original value
    [TestMethod]
    public void TestConversion_FEETToINCHESes()
    {
        double result = QuantityLength.Convert(1.0, LengthUnit.FEET, LengthUnit.INCHES);
        Assert.AreEqual(12.0, result, 0.000001);
    }

    //Testing addition of two lengths where one operand is null
    [TestMethod]
    public void TestAddition_NullSecondOperand()
    {
        QuantityLength firstValue = new QuantityLength(1.0, LengthUnit.FEET);
        try
        {
            firstValue.Add(null);
            Assert.Fail("Expected ArgumentException was not thrown.");
        }
        catch (ArgumentException)
        {
            // Test passes
        }
    }

    //Testing addition of two lengths where one has a negative value
    [TestMethod]
    public void TestAddition_NegativeValue()
    {
        QuantityLength firstValue = new QuantityLength(5.0, LengthUnit.FEET);
        QuantityLength secondValue = new QuantityLength(-2.0, LengthUnit.FEET);

        QuantityLength result = firstValue.Add(secondValue);

        Assert.IsTrue(result.Equals(new QuantityLength(3.0, LengthUnit.FEET)));
    }
    //Testing addition of zero to a length
    [TestMethod]
    public void TestAddition_WithZero()
    {
        QuantityLength firstValue = new QuantityLength(5.0, LengthUnit.FEET);
        QuantityLength secondValue = new QuantityLength(0.0, LengthUnit.INCHES);

        QuantityLength result = firstValue.Add(secondValue);

        Assert.IsTrue(result.Equals(new QuantityLength(5.0, LengthUnit.FEET)));
    }
    //Testing addition of two lengths in different units
    [TestMethod]
    public void TestAddition_CrossUnit_INCHESPlusFEET()
    {
        QuantityLength firstValue = new QuantityLength(12.0, LengthUnit.INCHES);
        QuantityLength secondValue = new QuantityLength(1.0, LengthUnit.FEET);

        QuantityLength result = firstValue.Add(secondValue);

        Assert.IsTrue(result.Equals(new QuantityLength(24.0, LengthUnit.INCHES)));
    }
    //Testing addition of two lengths in different units
    [TestMethod]
    public void TestAddition_CrossUnit_FEETPlusINCHES()
    {
        QuantityLength firstValue = new QuantityLength(1.0, LengthUnit.FEET);
        QuantityLength secondValue = new QuantityLength(12.0, LengthUnit.INCHES);

        QuantityLength result = firstValue.Add(secondValue);

        Assert.IsTrue(result.Equals(new QuantityLength(2.0, LengthUnit.FEET)));
    }
    //Testing addition of two lengths in the same unit
    [TestMethod]
    public void TestAddition_SameUnit_FEETPlusFEET()
    {
        QuantityLength firstValue = new QuantityLength(1.0, LengthUnit.FEET);
        QuantityLength secondValue = new QuantityLength(2.0, LengthUnit.FEET);

        QuantityLength result = firstValue.Add(secondValue);

        Assert.IsTrue(result.Equals(new QuantityLength(3.0, LengthUnit.FEET)));
    }
    //Testing addition of two lengths in the same unit
    [TestMethod]
    public void TestAddition_ExplicitTargetUnit_FEET()
    {
        QuantityLength firstValue = new QuantityLength(1.0, LengthUnit.FEET);
        QuantityLength secondValue = new QuantityLength(12.0, LengthUnit.INCHES);

        QuantityLength result =
            QuantityLength.AddTwoUnits_TargetUnit(firstValue, secondValue, LengthUnit.FEET);

        Assert.IsTrue(result.Equals(new QuantityLength(2.0, LengthUnit.FEET)));
    }
    //Testing addition of two lengths with an invalid target unit should throw an exception
    [TestMethod]
    public void TestAddition_ExplicitTargetUnit_INCHESes()
    {
        QuantityLength firstValue = new QuantityLength(1.0, LengthUnit.FEET);
        QuantityLength secondValue = new QuantityLength(12.0, LengthUnit.INCHES);

        QuantityLength result =
            QuantityLength.AddTwoUnits_TargetUnit(firstValue, secondValue, LengthUnit.INCHES);

        Assert.IsTrue(result.Equals(new QuantityLength(24.0, LengthUnit.INCHES)));
    }
    //Testing addition of two lengths with an invalid target unit should throw an exception
    [TestMethod]
    public void TestAddition_ExplicitTargetUnit_YARDSs()
    {
        QuantityLength firstValue = new QuantityLength(1.0, LengthUnit.FEET);
        QuantityLength secondValue = new QuantityLength(12.0, LengthUnit.INCHES);

        QuantityLength result =
            QuantityLength.AddTwoUnits_TargetUnit(firstValue, secondValue, LengthUnit.YARDS);

        Assert.IsTrue(result.Equals(new QuantityLength(0.666666, LengthUnit.YARDS)));
    }
    //Testing addition of two lengths with an invalid target unit should throw an exception
    [TestMethod]
    public void TestAddition_ExplicitTargetUnit_CENTIMETERS()
    {
        QuantityLength firstValue = new QuantityLength(2.54, LengthUnit.CENTIMETERS);
        QuantityLength secondValue = new QuantityLength(1.0, LengthUnit.INCHES);

        QuantityLength result =
            QuantityLength.AddTwoUnits_TargetUnit(firstValue, secondValue, LengthUnit.CENTIMETERS);

        Assert.IsTrue(result.Equals(new QuantityLength(5.08, LengthUnit.CENTIMETERS)));
    }
    //Testing addition of two lengths with an invalid target unit should throw an exception
    [TestMethod]
    public void TestAddition_ExplicitTargetUnit_Commutativity()
    {
        QuantityLength firstValue = new QuantityLength(1.0, LengthUnit.FEET);
        QuantityLength secondValue = new QuantityLength(12.0, LengthUnit.INCHES);

        QuantityLength r1 =
            QuantityLength.AddTwoUnits_TargetUnit(firstValue, secondValue, LengthUnit.YARDS);

        QuantityLength r2 =
            QuantityLength.AddTwoUnits_TargetUnit(secondValue, firstValue, LengthUnit.YARDS);

        Assert.IsTrue(r1.Equals(r2));
    }
    //Testing addition of two lengths with an invalid target unit should throw an exception
    [TestMethod]
    public void TestAddition_ExplicitTargetUnit_InvalidTarget()
    {
        QuantityLength firstValue = new QuantityLength(1.0, LengthUnit.FEET);
        QuantityLength secondValue = new QuantityLength(12.0, LengthUnit.INCHES);

        try
        {
            QuantityLength.AddTwoUnits_TargetUnit(firstValue, secondValue, (LengthUnit)999);
            Assert.Fail("Expected ArgumentException not thrown");
        }
        catch (ArgumentException)
        {
            // pass
        }
    }


    // ====================================================================================
    //Testing for WeightUnit and QuantityWeight classes

    [TestMethod]
    public void testEquality_KilogramToKilogram_SameValue()
    {
        QuantityWeight firstValue = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
        QuantityWeight secondValue = new QuantityWeight(1.0, WeightUnit.KILOGRAM);

        Assert.IsTrue(firstValue.Equals(secondValue));
    }

    [TestMethod]
    public void testEquality_KilogramToGram_EquivalentValue()
    {
        QuantityWeight firstValue = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
        QuantityWeight secondValue = new QuantityWeight(1000.0, WeightUnit.GRAM);

        Assert.IsTrue(firstValue.Equals(secondValue));
    }

    [TestMethod]
    public void testEquality_GramToKilogram_EquivalentValue()
    {
        QuantityWeight firstValue = new QuantityWeight(1000.0, WeightUnit.GRAM);
        QuantityWeight secondValue = new QuantityWeight(1.0, WeightUnit.KILOGRAM);

        Assert.IsTrue(firstValue.Equals(secondValue));
    }
    [TestMethod]
    public void testConversion_KilogramToGram()
    {
        QuantityWeight q = new QuantityWeight(1.0, WeightUnit.KILOGRAM);

        QuantityWeight result = q.ConvertTo(WeightUnit.GRAM);

        Assert.IsTrue(result.Equals(new QuantityWeight(1000.0, WeightUnit.GRAM)));
    }

    [TestMethod]
    public void testConversion_PoundToKilogram()
    {
        QuantityWeight q = new QuantityWeight(2.20462, WeightUnit.POUND);

        QuantityWeight result = q.ConvertTo(WeightUnit.KILOGRAM);

        Assert.IsTrue(result.Equals(new QuantityWeight(1.0, WeightUnit.KILOGRAM)));
    }
    [TestMethod]
    public void testAddition_SameUnit_KilogramPlusKilogram()
    {
        QuantityWeight firstValue = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
        QuantityWeight secondValue = new QuantityWeight(2.0, WeightUnit.KILOGRAM);

        QuantityWeight result = firstValue.Add(secondValue);

        Assert.IsTrue(result.Equals(new QuantityWeight(3.0, WeightUnit.KILOGRAM)));
    }

    [TestMethod]
    public void testAddition_CrossUnit_KilogramPlusGram()
    {
        QuantityWeight firstValue = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
        QuantityWeight secondValue = new QuantityWeight(1000.0, WeightUnit.GRAM);

        QuantityWeight result = firstValue.Add(secondValue);

        Assert.IsTrue(result.Equals(new QuantityWeight(2.0, WeightUnit.KILOGRAM)));
    }

    [TestMethod]
    public void testAddition_ExplicitTargetUnit_Kilogram()
    {
        QuantityWeight firstValue = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
        QuantityWeight secondValue = new QuantityWeight(1000.0, WeightUnit.GRAM);

        QuantityWeight result = firstValue.Add(secondValue, WeightUnit.GRAM);

        Assert.IsTrue(result.Equals(new QuantityWeight(2000.0, WeightUnit.GRAM)));
    }

    // ====================================================================================
    // Testing for VolumeUnit and QuantityVolume classes

    [TestMethod]
    public void testEquality_LitreToLitre_SameValue()
    {
        QuantityVolume firstValue = new QuantityVolume(1.0, VolumeUnit.LITRE);
        QuantityVolume secondValue = new QuantityVolume(1.0, VolumeUnit.LITRE);

        Assert.IsTrue(firstValue.Equals(secondValue));
    }

    [TestMethod]
    public void testEquality_LitreToMillilitre_EquivalentValue()
    {
        QuantityVolume firstValue = new QuantityVolume(1.0, VolumeUnit.LITRE);
        QuantityVolume secondValue = new QuantityVolume(1000.0, VolumeUnit.MILLILITRE);

        Assert.IsTrue(firstValue.Equals(secondValue));
    }

    [TestMethod]
    public void testEquality_MillilitreToLitre_EquivalentValue()
    {
        QuantityVolume firstValue = new QuantityVolume(1000.0, VolumeUnit.MILLILITRE);
        QuantityVolume secondValue = new QuantityVolume(1.0, VolumeUnit.LITRE);

        Assert.IsTrue(firstValue.Equals(secondValue));
    }

    [TestMethod]
    public void testConversion_LitreToMillilitre()
    {
        QuantityVolume q = new QuantityVolume(1.0, VolumeUnit.LITRE);

        QuantityVolume result = q.ConvertTo(VolumeUnit.MILLILITRE);

        Assert.IsTrue(result.Equals(new QuantityVolume(1000.0, VolumeUnit.MILLILITRE)));
    }

    [TestMethod]
    public void testConversion_GallonToLitre()
    {
        QuantityVolume q = new QuantityVolume(1.0, VolumeUnit.GALLON);

        QuantityVolume result = q.ConvertTo(VolumeUnit.LITRE);

        Assert.IsTrue(result.Equals(new QuantityVolume(3.78541, VolumeUnit.LITRE)));
    }

    [TestMethod]
    public void testAddition_SameUnit_LitrePlusLitre()
    {
        QuantityVolume firstValue = new QuantityVolume(1.0, VolumeUnit.LITRE);
        QuantityVolume secondValue = new QuantityVolume(2.0, VolumeUnit.LITRE);

        QuantityVolume result = firstValue.Add(secondValue);

        Assert.IsTrue(result.Equals(new QuantityVolume(3.0, VolumeUnit.LITRE)));
    }

    [TestMethod]
    public void testAddition_CrossUnit_LitrePlusMillilitre()
    {
        QuantityVolume firstValue = new QuantityVolume(1.0, VolumeUnit.LITRE);
        QuantityVolume secondValue = new QuantityVolume(1000.0, VolumeUnit.MILLILITRE);

        QuantityVolume result = firstValue.Add(secondValue);

        Assert.IsTrue(result.Equals(new QuantityVolume(2.0, VolumeUnit.LITRE)));
    }



    // ====================================================================================
    // UC12 – Subtraction Tests for Length

    [TestMethod]
    public void TestSubtraction_SameUnit_FEETMinusFEET()
    {
        QuantityLength firstValue = new QuantityLength(5.0, LengthUnit.FEET);
        QuantityLength secondValue = new QuantityLength(2.0, LengthUnit.FEET);

        QuantityLength result = firstValue.Subtract(secondValue);

        Assert.IsTrue(result.Equals(new QuantityLength(3.0, LengthUnit.FEET)));
    }

    [TestMethod]
    public void TestSubtraction_CrossUnit_FEETMinusINCHES()
    {
        QuantityLength firstValue = new QuantityLength(2.0, LengthUnit.FEET);
        QuantityLength secondValue = new QuantityLength(12.0, LengthUnit.INCHES);

        QuantityLength result = firstValue.Subtract(secondValue);

        Assert.IsTrue(result.Equals(new QuantityLength(1.0, LengthUnit.FEET)));
    }

    [TestMethod]
    public void TestSubtraction_CrossUnit_INCHESMinusFEET()
    {
        QuantityLength firstValue = new QuantityLength(24.0, LengthUnit.INCHES);
        QuantityLength secondValue = new QuantityLength(1.0, LengthUnit.FEET);

        QuantityLength result = firstValue.Subtract(secondValue);

        Assert.IsTrue(result.Equals(new QuantityLength(12.0, LengthUnit.INCHES)));
    }

    [TestMethod]
    public void TestSubtraction_TargetUnit_FEET()
    {
        QuantityLength firstValue = new QuantityLength(2.0, LengthUnit.FEET);
        QuantityLength secondValue = new QuantityLength(12.0, LengthUnit.INCHES);

        QuantityLength result = firstValue.Subtract(secondValue, LengthUnit.FEET);

        Assert.IsTrue(result.Equals(new QuantityLength(1.0, LengthUnit.FEET)));
    }

    [TestMethod]
    public void TestSubtraction_TargetUnit_INCHES()
    {
        QuantityLength firstValue = new QuantityLength(2.0, LengthUnit.FEET);
        QuantityLength secondValue = new QuantityLength(12.0, LengthUnit.INCHES);

        QuantityLength result = firstValue.Subtract(secondValue, LengthUnit.INCHES);

        Assert.IsTrue(result.Equals(new QuantityLength(12.0, LengthUnit.INCHES)));
    }


    // ====================================================================================
    // UC12 – Division Tests for Length

    [TestMethod]
    public void TestDivision_SameUnit()
    {
        QuantityLength firstValue = new QuantityLength(10.0, LengthUnit.FEET);
        QuantityLength secondValue = new QuantityLength(2.0, LengthUnit.FEET);

        double result = firstValue.Divide(secondValue);

        Assert.AreEqual(5.0, result, 0.000001);
    }

    [TestMethod]
    public void TestDivision_CrossUnit()
    {
        QuantityLength firstValue = new QuantityLength(24.0, LengthUnit.INCHES);
        QuantityLength secondValue = new QuantityLength(1.0, LengthUnit.FEET);

        double result = firstValue.Divide(secondValue);

        Assert.AreEqual(2.0, result, 0.000001);
    }

    [TestMethod]
    public void TestDivision_ByZero()
    {
        QuantityLength firstValue = new QuantityLength(10.0, LengthUnit.FEET);
        QuantityLength secondValue = new QuantityLength(0.0, LengthUnit.FEET);

        try
        {
            firstValue.Divide(secondValue);
            Assert.Fail("Expected ArithmeticException not thrown");
        }
        catch (ArithmeticException)
        {
            // pass
        }
    }


    // ====================================================================================
    // UC12 – Subtraction Tests for Weight

    [TestMethod]
    public void testSubtraction_KilogramMinusGram()
    {
        QuantityWeight firstValue = new QuantityWeight(2.0, WeightUnit.KILOGRAM);
        QuantityWeight secondValue = new QuantityWeight(500.0, WeightUnit.GRAM);

        QuantityWeight result = firstValue.Subtract(secondValue);

        Assert.IsTrue(result.Equals(new QuantityWeight(1.5, WeightUnit.KILOGRAM)));
    }


    // ====================================================================================
    // UC12 – Division Tests for Volume

    [TestMethod]
    public void testDivision_LitreByMillilitre()
    {
        QuantityVolume firstValue = new QuantityVolume(2.0, VolumeUnit.LITRE);
        QuantityVolume secondValue = new QuantityVolume(500.0, VolumeUnit.MILLILITRE);

        double result = firstValue.Divide(secondValue);

        Assert.AreEqual(4.0, result, 0.000001);
    }

    //UC14-

 [TestMethod]
public void GivenCelsius_WhenConvertedToFahrenheit_ShouldReturnCorrectValue()
{
    var temp = new Quantity<TemperatureUnit>(0, TemperatureUnit.CELSIUS);

    var result = temp.ConvertTo(TemperatureUnit.FAHRENHEIT);

    Assert.AreEqual(
        new Quantity<TemperatureUnit>(32, TemperatureUnit.FAHRENHEIT),
        result);
}
[TestMethod]
public void GivenFahrenheit_WhenConvertedToCelsius_ShouldReturnCorrectValue()
{
    var temp = new Quantity<TemperatureUnit>(32, TemperatureUnit.FAHRENHEIT);

    var result = temp.ConvertTo(TemperatureUnit.CELSIUS);

    Assert.AreEqual(
        new Quantity<TemperatureUnit>(0, TemperatureUnit.CELSIUS),
        result);
}
[TestMethod]
public void GivenTwoTemperatures_WhenAdded_ShouldThrowException()
{
    var t1 = new Quantity<TemperatureUnit>(30, TemperatureUnit.CELSIUS);
    var t2 = new Quantity<TemperatureUnit>(20, TemperatureUnit.CELSIUS);

    Assert.ThrowsException<UnsupportedOperationException>(() =>
    {
        t1.Add(t2, TemperatureUnit.CELSIUS);
    });
}


//uc16 ado.net


// [TestMethod]
// public void UC16_SaveOperation_ShouldInsertRecordInDatabase()
// {
//     // Arrange
//     IConfiguration config = new ConfigurationBuilder()
//         .AddJsonFile("appsettings.json")
//         .Build();

//     IQuantityMeasurementRepository repository =
//         new QuantityMeasurementDatabaseRepository(config);

//     QuantityMeasurementEntity entity = new QuantityMeasurementEntity
//     {
//         FirstValue = 5,
//         FirstUnit = "FEET",
//         SecondValue = 10,
//         SecondUnit = "FEET",
//         Operation = "ADD",
//         Result = 15,
//         MeasurementType = "Length"
//     };

//     // Act
//     repository.Save(entity);

//     var records = repository.GetAll();

//     // Assert
//     Assert.IsTrue(records.Count > 0);
// }
// [TestMethod]
// public void UC16_GetByOperation_ShouldReturnCorrectRecords()
// {
//     // Arrange
//     IConfiguration config = new ConfigurationBuilder()
//         .AddJsonFile("appsettings.json")
//         .Build();

//     IQuantityMeasurementRepository repository =
//         new QuantityMeasurementDatabaseRepository(config);

//     // Act
//     var result = repository.GetByOperation("ADD");

//     // Assert
//     Assert.IsNotNull(result);
// }
// [TestMethod]
// public void UC16_GetByMeasurementType_ShouldReturnCorrectRecords()
// {
//     // Arrange
//     IConfiguration config = new ConfigurationBuilder()
//         .AddJsonFile("appsettings.json")
//         .Build();

//     IQuantityMeasurementRepository repository =
//         new QuantityMeasurementDatabaseRepository(config);

//     // Act
//     var result = repository.GetByMeasurementType("Length");

//     // Assert
//     Assert.IsNotNull(result);
// }
// [TestMethod]
// public void UC16_GetTotalCount_ShouldReturnTotalDatabaseRecords()
// {
//     // Arrange
//     IConfiguration config = new ConfigurationBuilder()
//         .AddJsonFile("appsettings.json")
//         .Build();

//     IQuantityMeasurementRepository repository =
//         new QuantityMeasurementDatabaseRepository(config);

//     // Act
//     int count = repository.GetTotalCount();

//     // Assert
//     Assert.IsTrue(count >= 0);
// }
// [TestMethod]
// public void UC16_DeleteAll_ShouldRemoveAllRecordsFromDatabase()
// {
//     // Arrange
//     IConfiguration config = new ConfigurationBuilder()
//         .AddJsonFile("appsettings.json")
//         .Build();

//     IQuantityMeasurementRepository repository =
//         new QuantityMeasurementDatabaseRepository(config);

//     // Act
//     repository.DeleteAll();

//     int count = repository.GetTotalCount();

//     // Assert
//     Assert.AreEqual(0, count);
// }
// }

[TestMethod]
public void UC16_SaveOperation_ShouldInsertRecordInCache()
{
    // Arrange
    IQuantityMeasurementRepository repository =
        new QuantityMeasurementCacheRepository();

    QuantityMeasurementEntity entity = new QuantityMeasurementEntity
    {
        FirstValue = 5,
        FirstUnit = "FEET",
        SecondValue = 10,
        SecondUnit = "FEET",
        Operation = "ADD",
        Result = 15,
        MeasurementType = "Length"
    };

    // Act
    repository.Save(entity);
    var records = repository.GetAll();

    // Assert
    Assert.IsTrue(records.Count > 0);
}

[TestMethod]
public void UC16_GetByOperation_ShouldReturnCorrectRecords()
{
    // Arrange
    IQuantityMeasurementRepository repository =
        new QuantityMeasurementCacheRepository();

    repository.Save(new QuantityMeasurementEntity
    {
        FirstValue = 5,
        FirstUnit = "FEET",
        SecondValue = 10,
        SecondUnit = "FEET",
        Operation = "ADD",
        Result = 15,
        MeasurementType = "Length"
    });

    // Act
    var result = repository.GetByOperation("ADD");

    // Assert
    Assert.IsNotNull(result);
}

[TestMethod]
public void UC16_GetByMeasurementType_ShouldReturnCorrectRecords()
{
    // Arrange
    IQuantityMeasurementRepository repository =
        new QuantityMeasurementCacheRepository();

    repository.Save(new QuantityMeasurementEntity
    {
        FirstValue = 5,
        FirstUnit = "FEET",
        SecondValue = 10,
        SecondUnit = "FEET",
        Operation = "ADD",
        Result = 15,
        MeasurementType = "Length"
    });

    // Act
    var result = repository.GetByMeasurementType("Length");

    // Assert
    Assert.IsNotNull(result);
}

[TestMethod]
public void UC16_GetTotalCount_ShouldReturnTotalCacheRecords()
{
    // Arrange
    IQuantityMeasurementRepository repository =
        new QuantityMeasurementCacheRepository();

    repository.Save(new QuantityMeasurementEntity
    {
        FirstValue = 5,
        FirstUnit = "FEET",
        SecondValue = 10,
        SecondUnit = "FEET",
        Operation = "ADD",
        Result = 15,
        MeasurementType = "Length"
    });

    // Act
    int count = repository.GetTotalCount();

    // Assert
    Assert.IsTrue(count > 0);
}

[TestMethod]
public void UC16_DeleteAll_ShouldRemoveAllRecordsFromCache()
{
    // Arrange
    IQuantityMeasurementRepository repository =
        new QuantityMeasurementCacheRepository();

    repository.Save(new QuantityMeasurementEntity
    {
        FirstValue = 5,
        FirstUnit = "FEET",
        SecondValue = 10,
        SecondUnit = "FEET",
        Operation = "ADD",
        Result = 15,
        MeasurementType = "Length"
    });

    // Act
    repository.DeleteAll();
    int count = repository.GetTotalCount();

    // Assert
    Assert.AreEqual(0, count);
}}