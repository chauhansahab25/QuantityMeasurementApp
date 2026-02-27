using System;

namespace QuantityMeasurementApp.Model
{
    public class QuantityWeight
    {
        private readonly double value;
        private readonly WeightUnit unit;

        public double Value => value;
        public WeightUnit Unit => unit;

        public QuantityWeight(double value, WeightUnit unit)
        {
            if (Double.IsNaN(value) || Double.IsInfinity(value))
                throw new ArgumentException("Value must be finite number.");
            this.value = value;
            this.unit = unit;
        }

        private double ConvertToBaseUnit()
        {
            return unit.ConvertToBaseUnit(value);
        }

        public static double Convert(double value, WeightUnit source, WeightUnit target)
        {
            if (source == target)
                return value;

            double baseValue = source.ConvertToBaseUnit(value);
            return target.ConvertFromBaseUnit(baseValue);
        }

        public QuantityWeight Add(QuantityWeight other)
        {
            if (other == null)
                throw new ArgumentException("Second operand cannot be null");

            double thisInBase = this.ConvertToBaseUnit();
            double otherInBase = other.ConvertToBaseUnit();
            double sumInBase = thisInBase + otherInBase;
            double resultValue = this.unit.ConvertFromBaseUnit(sumInBase);

            return new QuantityWeight(resultValue, this.unit);
        }

        public static QuantityWeight Add(QuantityWeight w1, QuantityWeight w2)
        {
            if (w1 == null || w2 == null)
                throw new ArgumentException("Operands cannot be null");

            return w1.Add(w2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (this == obj) return true;
            if (!(obj is QuantityWeight)) return false;
            QuantityWeight other = (QuantityWeight)obj;
            double thisInBase = this.ConvertToBaseUnit();
            double otherInBase = other.ConvertToBaseUnit();
            return Math.Abs(thisInBase - otherInBase) < 0.000001;
        }

        public override int GetHashCode()
        {
            return ConvertToBaseUnit().GetHashCode();
        }

        public override string ToString()
        {
            return value + " " + unit;
        }
    }
}
