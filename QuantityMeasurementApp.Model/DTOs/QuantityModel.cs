using System;

namespace QuantityMeasurementWebAPI.DTOs
{
    public class QuantityModel<U> where U : Enum
    {
        public double Value { get; set; }
        public U Unit { get; set; }

        public QuantityModel(double value, U unit)
        {
            Value = value;
            Unit = unit;
        }
    }
}
