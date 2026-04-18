namespace QuantityMeasurementWebApi.DTOs;

public class HistoryRecordDto
{
    public int Id { get; set; }
    public string Operation { get; set; } = string.Empty;
    public string MeasurementType { get; set; } = string.Empty;
    public double FirstValue { get; set; }
    public string FirstUnit { get; set; } = string.Empty;
    public double? SecondValue { get; set; }
    public string? SecondUnit { get; set; }
    public double? Result { get; set; }
    public string? ResultString { get; set; }
    public string? ErrorMessage { get; set; }
    public bool IsError { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class HistoryResponseDto
{
    public List<HistoryRecordDto> Data { get; set; } = new();
    public int Total { get; set; }
    public int TotalPages { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}
