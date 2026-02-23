using System;
using System.Collections.Generic;

namespace QuantityMeasurementApp.Model
{
    public class Quantity
    {
        public double Value { get; }
        public Unit Unit { get; }

        private const double EPSILON = 1e-6;

        private static readonly Dictionary<Unit, double> conversionToFeet =
            new Dictionary<Unit, double>
        {
            { Unit.FEET, 1.0 },
            { Unit.INCH, 1.0 / 12.0 },
            { Unit.YARD, 3.0 },
            { Unit.CENTIMETER, 1.0 / 30.48 }
        };

        public Quantity(double value, Unit unit)
        {
            if (!Enum.IsDefined(typeof(Unit), unit))
                throw new ArgumentException("Invalid Unit");

            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid numeric value");

            Value = value;
            Unit = unit;
        }

        public double ConvertTo(Unit targetUnit)
        {
            if (!Enum.IsDefined(typeof(Unit), targetUnit))
                throw new ArgumentException("Invalid Target Unit");

            double valueInFeet = Value * conversionToFeet[Unit];

            return valueInFeet / conversionToFeet[targetUnit];
        }

        
        public override bool Equals(object obj)
        {
            if (obj is not Quantity other)
                return false;

            double thisInFeet = Value * conversionToFeet[Unit];
            double otherInFeet = other.Value * conversionToFeet[other.Unit];

            return Math.Abs(thisInFeet - otherInFeet) < EPSILON;
        }

        public override int GetHashCode()
        {
            return (Value, Unit).GetHashCode();
        }
    }
}