 using QuantityMeasurementModelLayer.Enums;
 public static class LengthUnitExtensions
    {
        public static double GetConversionFactor(this LengthUnit unit)
        {
            switch (unit)
            {
                case LengthUnit.FEET: return 1.0;
                case LengthUnit.INCHES: return 1.0 / 12.0;
                case LengthUnit.YARDS: return 3.0;
                case LengthUnit.CENTIMETERS: return 1.0 / 30.48;
                default: throw new ArgumentException();
            }
        }

        public static double ConvertToBaseUnit(this LengthUnit unit, double value)
        {
            return value * unit.GetConversionFactor();
        }

        public static double ConvertFromBaseUnit(this LengthUnit unit, double baseValue)
        {
            return baseValue / unit.GetConversionFactor();
        }

        public static string GetUnitName(this LengthUnit unit)
        {
            return unit.ToString();
        }
    }
