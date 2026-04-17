using QuantityMeasurementModelLayer.DTO;

namespace QuantityMeasurementService.DTOs
{
    public class QuantityMeasurementRequestDto
    {
        public double FirstValue { get; set; }
        public string FirstUnit { get; set; } = string.Empty;
        public double SecondValue { get; set; }
        public string SecondUnit { get; set; } = string.Empty;
    }

    public class QuantityMeasurementOperationResultDto
    {
        public double Result { get; set; }
        public string? ResultUnit { get; set; }
        public string Operation { get; set; } = string.Empty;
        public QuantityDTO? FirstQuantity { get; set; }
        public QuantityDTO? SecondQuantity { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class QuantityMeasurementConvertRequestDto
    {
        public double Value { get; set; }
        public string FromUnit { get; set; } = string.Empty;
        public string ToUnit { get; set; } = string.Empty;
    }

    public class QuantityMeasurementHistoryDto
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
    }
}
