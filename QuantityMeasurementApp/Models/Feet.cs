using System;

namespace QuantityMeasurementApp
{
        public class Feet
        {
            private readonly double value;

            public Feet(double value)
            {
                this.value = value;
            }

            public double Value => value;

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(this, obj))
                    return true;

            
                if (obj is null)
                    return false;

                if (obj is not Feet other)
                    return false;


                return double.Equals(this.value, other.value) ;
            }

            public override int GetHashCode()
            {
                return value.GetHashCode();
            }
        }

   
    }