namespace QuantityMeasurementWebApi.DTOs;

public class QuantityMeasurementResponseDto
{
    public int Id { get; set; }
    public double FirstValue { get; set; }
    public string FirstUnit { get; set; } = string.Empty;
    public double SecondValue { get; set; }
    public string SecondUnit { get; set; } = string.Empty;
    public string Operation { get; set; } = string.Empty;
    public double Result { get; set; }
    public string MeasurementType { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsError { get; set; }
    public string? ErrorMessage { get; set; }
    public string? ResultString { get; set; }
}
