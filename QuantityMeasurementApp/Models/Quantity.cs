using System;

namespace QuantityMeasurementApp.Model
{
    public class Quantity : IEquatable<Quantity>
    {
        public double Value { get; }
        public Unit Unit { get; }

        public Quantity(double value, Unit unit)
        {
            if (value < 0)
                throw new ArgumentException("Value cannot be negative");

            
            if (!Enum.IsDefined(typeof(Unit), unit))
                throw new ArgumentException("Invalid unit");

            Value = value;
            Unit = unit;
        }

        private double ToInches()
        {
            return Unit switch
            {
                Unit.FEET => Value * 12,
                Unit.INCH => Value,
                _ => throw new ArgumentException("Invalid unit")
            };
        }

        public bool Equals(Quantity? other)
        {
            if (other is null)
                return false;

            return this.ToInches() == other.ToInches();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Quantity);
        }

        public override int GetHashCode()
        {
            return ToInches().GetHashCode();
        }

        public static bool operator ==(Quantity left, Quantity right)
        {
            if (left is null)
                return right is null;

            return left.Equals(right);
        }

        public static bool operator !=(Quantity left, Quantity right)
        {
            return !(left == right);
        }
    }
}