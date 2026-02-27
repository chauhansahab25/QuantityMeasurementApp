
namespace QuantityMeasurementApp.Model;
public class Feet
{
    private readonly double value;

    public Feet(double value)
    {
        this.value = value;
    }
    public override bool Equals(object obj)
    {
        if (ReferenceEquals(this, obj)) { return true; }
        if (obj == null)
        {
            return false;
        }
        if (obj.GetType() != typeof(Feet))
        {
            return false;
        }
        Feet other = (Feet)obj;
        return this.value.Equals(other.value);
    }
    public override int GetHashCode()
    {
        return value.GetHashCode();
    }
}
