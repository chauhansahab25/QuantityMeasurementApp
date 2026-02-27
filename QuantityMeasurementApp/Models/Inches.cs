namespace QuantityMeasurementApp.Model;

public class Inches
{
    private readonly double value;
    public Inches(double value)
    {
        this.value = value;
    }
    public override bool Equals(object obj)
    {
        if (ReferenceEquals(this, obj)) return true;
        if (obj == null || GetType() != obj.GetType()) return false;
        Inches other = (Inches)obj;
        return value == other.value;
    }
}