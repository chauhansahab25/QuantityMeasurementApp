
namespace QuantityMeasurementApp{
 public class Inches
        {
            private readonly double value;

            public Inches(double value)
            {
                this.value = value;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(this, obj))
                    return true;

                if (obj == null || GetType() != obj.GetType())
                    return false;

                Inches other = (Inches)obj;

                return double.Equals(this.value, other.value);
            }

            public override int GetHashCode()
            {
                return value.GetHashCode();
            }
        }
 }
