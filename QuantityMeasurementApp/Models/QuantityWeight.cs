using System;

namespace QuantityMeasurementApp.Model
{
    public class QuantityWeight : Quantity<WeightUnit>
    {
        public QuantityWeight(double value, WeightUnit unit) : base(value, unit)
        {
        }

        public QuantityWeight Add(QuantityWeight other)
        {
            var result = base.Add(other);
            return new QuantityWeight(result.Value, result.Unit);
        }

        public static QuantityWeight Add(QuantityWeight w1, QuantityWeight w2)
        {
            var result = Quantity<WeightUnit>.Add(w1, w2);
            return new QuantityWeight(result.Value, result.Unit);
        }
    }
}
