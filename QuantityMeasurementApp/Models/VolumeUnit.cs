namespace QuantityMeasurementApp.Model
{
    public enum VolumeUnit
    {
        LITRE,
        MILLILITRE,
        GALLON
    }

    public static class VolumeUnitExtensions
    {
        private const double GALLON_TO_LITRE = 3.78541;

        public static double ConvertToBaseUnit(this VolumeUnit unit, double value)
        {
            return unit switch
            {
                VolumeUnit.LITRE => value,
                VolumeUnit.MILLILITRE => value * 0.001,
                VolumeUnit.GALLON => value * GALLON_TO_LITRE,
                _ => throw new ArgumentException("Invalid unit")
            };
        }

        public static double ConvertFromBaseUnit(this VolumeUnit unit, double baseValue)
        {
            return unit switch
            {
                VolumeUnit.LITRE => baseValue,
                VolumeUnit.MILLILITRE => baseValue * 1000,
                VolumeUnit.GALLON => baseValue / GALLON_TO_LITRE,
                _ => throw new ArgumentException("Invalid unit")
            };
        }
    }
}
