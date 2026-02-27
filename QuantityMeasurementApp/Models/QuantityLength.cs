
using System;

namespace QuantityMeasurementApp.Model;
    public class QuantityLength
    {
        private readonly double value;
        private readonly LengthUnit unit;

        public QuantityLength(double value, LengthUnit unit)
        {
            this.value = value;
            this.unit = unit;
        }

        private double ConvertToFeet()
        {
            if (unit == LengthUnit.Feet)
                return value;
            if (unit == LengthUnit.Inch)
                return value / 12;
            throw new ArgumentException("Invalid unit");
        }
        public override bool Equals(object obj)
        {
            if (obj == null)return false;
            if (this == obj)return true;
            if (!(obj is QuantityLength))return false;
            QuantityLength other = (QuantityLength)obj;
            double thisInFeet = this.ConvertToFeet();
            double otherInFeet = other.ConvertToFeet();
            return thisInFeet == otherInFeet;
        }

        public override int GetHashCode()
        {
            return ConvertToFeet().GetHashCode();
        }
    }
