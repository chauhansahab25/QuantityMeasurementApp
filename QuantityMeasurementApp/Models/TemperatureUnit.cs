using System;

namespace QuantityMeasurementApp.Model
{
    public enum TemperatureUnit
    {
        CELSIUS,
        FAHRENHEIT
    }

    public static class TemperatureUnitExtensions
    {
        public static double ConvertToBaseUnit(this TemperatureUnit unit, double value)
        {
            return unit switch
            {
                TemperatureUnit.CELSIUS => value,
                TemperatureUnit.FAHRENHEIT => (value - 32) * 5 / 9,
                _ => throw new ArgumentException("Invalid unit")
            };
        }

        public static double ConvertFromBaseUnit(this TemperatureUnit unit, double baseValue)
        {
            return unit switch
            {
                TemperatureUnit.CELSIUS => baseValue,
                TemperatureUnit.FAHRENHEIT => (baseValue * 9 / 5) + 32,
                _ => throw new ArgumentException("Invalid unit")
            };
        }
    }
}
