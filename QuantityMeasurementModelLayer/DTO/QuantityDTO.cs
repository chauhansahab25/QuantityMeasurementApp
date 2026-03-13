namespace QuantityMeasurementModelLayer.DTO;

public class QuantityDTO
{
    public double Value { get; set; }
    public string Unit { get; set; }

    public QuantityDTO(double value, string unit)
    {
        Value = value;
        Unit = unit;
    }
}