using System;

namespace QuantityMeasurementApp.Model
{
    public class Quantity<U> where U : Enum
    {
        private readonly double value;
        private readonly U unit;

        public double Value => value;
        public U Unit => unit;

        public Quantity(double value, U unit)
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

        public static double Convert(double value, U source, U target)
        {
            if (source.Equals(target))
                return value;

            double baseValue = source.ConvertToBaseUnit(value);
            return target.ConvertFromBaseUnit(baseValue);
        }

        public Quantity<U> Add(Quantity<U> other)
        {
            if (other == null)
                throw new ArgumentException("Second operand cannot be null");

            double thisInBase = this.ConvertToBaseUnit();
            double otherInBase = other.ConvertToBaseUnit();
            double sumInBase = thisInBase + otherInBase;
            double resultValue = this.unit.ConvertFromBaseUnit(sumInBase);

            return new Quantity<U>(resultValue, this.unit);
        }

        public static Quantity<U> Add(Quantity<U> q1, Quantity<U> q2)
        {
            if (q1 == null || q2 == null)
                throw new ArgumentException("Operands cannot be null");

            return q1.Add(q2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (this == obj) return true;
            if (!(obj is Quantity<U>)) return false;
            Quantity<U> other = (Quantity<U>)obj;
            if (this.unit.GetType() != other.unit.GetType()) return false;
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
