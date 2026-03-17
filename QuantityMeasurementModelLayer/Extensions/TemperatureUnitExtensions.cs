using QuantityMeasurementModelLayer.Enums;
using QuantityMeasurementModelLayer.Exceptions;

namespace QuantityMeasurementModelLayer.Extensions;

public static class TemperatureUnitExtensions
{
    public static double ConvertToBaseUnit(this TemperatureUnit unit, double value)
    {
        if (unit == TemperatureUnit.CELSIUS)
            return value;

        if (unit == TemperatureUnit.FAHRENHEIT)
            return (value - 32) * 5 / 9;

        throw new ArgumentException("Invalid Temperature Unit");
    }

    public static double ConvertFromBaseUnit(this TemperatureUnit unit, double baseValue)
    {
        if (unit == TemperatureUnit.CELSIUS)
            return baseValue;

        if (unit == TemperatureUnit.FAHRENHEIT)
            return (baseValue * 9 / 5) + 32;

        throw new ArgumentException("Invalid Temperature Unit");
    }

    public static void ValidateOperationSupport(this TemperatureUnit unit, string operation)
    {
        throw new UnsupportedOperationException(
            $"Temperature does not support {operation}");
    }

    public static bool SupportsArithmetic(this TemperatureUnit unit)
    {
        return false; // Temperature units do not support arithmetic operations
    }
}
