using System;

namespace QuantityMeasurementApp.Model
{
    public interface IMeasurable<T> where T : Enum
    {
    }

    public static class IMeasurableExtensions
    {
        public static double ConvertToBaseUnit<T>(this T unit, double value) where T : Enum
        {
            if (unit is Unit lengthUnit)
                return lengthUnit.ConvertToBaseUnit(value);
            if (unit is WeightUnit weightUnit)
                return weightUnit.ConvertToBaseUnit(value);
            throw new ArgumentException("Unsupported unit type");
        }

        public static double ConvertFromBaseUnit<T>(this T unit, double baseValue) where T : Enum
        {
            if (unit is Unit lengthUnit)
                return lengthUnit.ConvertFromBaseUnit(baseValue);
            if (unit is WeightUnit weightUnit)
                return weightUnit.ConvertFromBaseUnit(baseValue);
            throw new ArgumentException("Unsupported unit type");
        }
    }
}
