namespace QuantityMeasurementModelLayer.Models;

public class QuantityModel<U> where U : struct
{
    public double Value { get; set; }
    public U Unit { get; set; }

    public QuantityModel(double value, U unit)
    {
        Value = value;
        Unit = unit;
    }
}