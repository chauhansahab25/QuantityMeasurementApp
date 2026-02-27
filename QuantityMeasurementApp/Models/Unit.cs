namespace QuantityMeasurementApp.Model
{
    public enum Unit
    {
        FEET,
        INCH,
        YARD,
        CENTIMETER
    }

    public static class UnitExtensions
    {
        public static double ConvertToBaseUnit(this Unit unit, double value)
        {
            return unit switch
            {
                Unit.FEET => value,
                Unit.INCH => value / 12,
                Unit.YARD => value * 3,
                Unit.CENTIMETER => value / 30.48,
                _ => throw new ArgumentException("Invalid unit")
            };
        }

        public static double ConvertFromBaseUnit(this Unit unit, double baseValue)
        {
            return unit switch
            {
                Unit.FEET => baseValue,
                Unit.INCH => baseValue * 12,
                Unit.YARD => baseValue / 3,
                Unit.CENTIMETER => baseValue * 30.48,
                _ => throw new ArgumentException("Invalid unit")
            };
        }
    }
}