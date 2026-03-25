namespace QuantityMeasurementWebApi.DTOs;

public class QuantityMeasurementOperationResultDto
{
    public double Result { get; set; }
    public string ResultString { get; set; } = string.Empty;
    public bool IsError { get; set; }
    public string? ErrorMessage { get; set; }
    public string Operation { get; set; } = string.Empty;
    public string MeasurementType { get; set; } = string.Empty;
}
