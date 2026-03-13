using QuantityMeasurementModelLayer.Enums;
    public static class WeightUnitExtensions
    {
        public static double GetConversionFactor(this WeightUnit unit)
        {
            switch (unit)
            {
                case WeightUnit.KILOGRAM: return 1.0;
                case WeightUnit.GRAM: return 0.001;
                case WeightUnit.POUND: return 0.45359237;
                default: throw new ArgumentException();
            }
        }

        public static double ConvertToBaseUnit(this WeightUnit unit, double value)
        {
            return value * unit.GetConversionFactor();
        }

        public static double ConvertFromBaseUnit(this WeightUnit unit, double baseValue)
        {
            return baseValue / unit.GetConversionFactor();
        }

        public static string GetUnitName(this WeightUnit unit)
        {
            return unit.ToString();
        }
    }
