using System;
using QuantityMeasurementModelLayer.Enums;

public static class VolumeUnitExtensions
{
    public static double ToBaseUnit(this VolumeUnit unit)
    {
        switch (unit)
        {
            case VolumeUnit.LITRE:
                return 1.0;

            case VolumeUnit.MILLILITRE:
                return 0.001;

            case VolumeUnit.GALLON:
                return 3.78541;

            default:
                throw new ArgumentException("Invalid Volume Unit");
        }
    }

    public static double ConvertToBaseUnit(this VolumeUnit unit, double value)
    {
        switch (unit)
        {
            case VolumeUnit.LITRE:
                return value;

            case VolumeUnit.MILLILITRE:
                return value * 0.001;

            case VolumeUnit.GALLON:
                return value * 3.78541;

            default:
                throw new ArgumentException("Invalid Volume Unit");
        }
    }

    public static double ConvertFromBaseUnit(this VolumeUnit unit, double baseValue)
    {
        switch (unit)
        {
            case VolumeUnit.LITRE:
                return baseValue;

            case VolumeUnit.MILLILITRE:
                return baseValue / 0.001;

            case VolumeUnit.GALLON:
                return baseValue / 3.78541;

            default:
                throw new ArgumentException("Invalid Volume Unit");
        }
    }
}