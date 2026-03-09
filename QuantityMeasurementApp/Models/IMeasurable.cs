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
            if (unit is VolumeUnit volumeUnit)
                return volumeUnit.ConvertToBaseUnit(value);
            if (unit is TemperatureUnit tempUnit)
                return tempUnit.ConvertToBaseUnit(value);
            throw new ArgumentException("Unsupported unit type");
        }

        public static double ConvertFromBaseUnit<T>(this T unit, double baseValue) where T : Enum
        {
            if (unit is Unit lengthUnit)
                return lengthUnit.ConvertFromBaseUnit(baseValue);
            if (unit is WeightUnit weightUnit)
                return weightUnit.ConvertFromBaseUnit(baseValue);
            if (unit is VolumeUnit volumeUnit)
                return volumeUnit.ConvertFromBaseUnit(baseValue);
            if (unit is TemperatureUnit tempUnit)
                return tempUnit.ConvertFromBaseUnit(baseValue);
            throw new ArgumentException("Unsupported unit type");
        }

        public static bool SupportsArithmetic<T>(this T unit) where T : Enum
        {
            if (unit is TemperatureUnit)
                return false;
            return true;
        }

        public static void ValidateOperationSupport<T>(this T unit, string operation) where T : Enum
        {
            if (unit is TemperatureUnit)
            {
                throw new NotSupportedException($"Temperature does not support {operation} operation. Temperature measurements can only be compared and converted.");
            }
        }
    }
}
