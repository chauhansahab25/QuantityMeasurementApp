using System;

namespace QuantityMeasurementApp.Model
{
    public class Quantity
    {
        private readonly double value;
        private readonly Unit unit;

        private const double TOLERANCE = 0.0001;

        public Quantity(double value, Unit unit)
        {
            if (value < 0)
                throw new ArgumentException("Length cannot be negative");

            if (!Enum.IsDefined(typeof(Unit), unit))
                throw new ArgumentException("Invalid unit provided");

            this.value = value;
            this.unit = unit;
        }

        private double ToInches()
        {
            return unit switch
            {
                Unit.Feet => value * 12,
                Unit.Inches => value,
                Unit.Yards => value * 36,
                Unit.Centimeters => value * 0.393701,
                _ => throw new InvalidOperationException("Invalid unit")
            };
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            if (obj is not Quantity other)
                return false;

            return Math.Abs(this.ToInches() - other.ToInches()) < TOLERANCE;
        }

        public override int GetHashCode()
        {
            // Round to match tolerance to keep equality contract safe
            return Math.Round(ToInches(), 4).GetHashCode();
        }
    }
}