using System;

namespace QuantityMeasurementApp.Model
{
    public enum WeightUnit
    {
        KILOGRAM,
        GRAM,
        POUND
    }

    public static class WeightUnitExtensions
    {
        private const double POUND_TO_KG = 0.453592;

        public static double ConvertToBaseUnit(this WeightUnit unit, double value)
        {
            return unit switch
            {
                WeightUnit.KILOGRAM => value,
                WeightUnit.GRAM => value * 0.001,
                WeightUnit.POUND => value * POUND_TO_KG,
                _ => throw new ArgumentException("Invalid unit")
            };
        }

        public static double ConvertFromBaseUnit(this WeightUnit unit, double baseValue)
        {
            return unit switch
            {
                WeightUnit.KILOGRAM => baseValue,
                WeightUnit.GRAM => baseValue * 1000,
                WeightUnit.POUND => baseValue / POUND_TO_KG,
                _ => throw new ArgumentException("Invalid unit")
            };
        }
    }
}
