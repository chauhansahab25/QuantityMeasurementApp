using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurementWebApi.DTOs;

public class QuantityMeasurementRequestDto
{
    [Required(ErrorMessage = "First value is required")]
    [Range(double.MinValue, double.MaxValue, ErrorMessage = "First value must be a valid number")]
    public double FirstValue { get; set; }

    [Required(ErrorMessage = "First unit is required")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "First unit must be between 1 and 50 characters")]
    public string FirstUnit { get; set; } = string.Empty;

    [Required(ErrorMessage = "Second value is required")]
    [Range(double.MinValue, double.MaxValue, ErrorMessage = "Second value must be a valid number")]
    public double SecondValue { get; set; }

    [Required(ErrorMessage = "Second unit is required")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Second unit must be between 1 and 50 characters")]
    public string SecondUnit { get; set; } = string.Empty;

    [Required(ErrorMessage = "Operation is required")]
    [RegularExpression("^(ADD|SUBTRACT|MULTIPLY|DIVIDE|COMPARE|CONVERT)$", ErrorMessage = "Operation must be one of: ADD, SUBTRACT, MULTIPLY, DIVIDE, COMPARE, CONVERT")]
    public string Operation { get; set; } = string.Empty;

    [Required(ErrorMessage = "Measurement type is required")]
    [RegularExpression("^(LengthUnit|VolumeUnit|WeightUnit|TemperatureUnit)$", ErrorMessage = "Measurement type must be one of: LengthUnit, VolumeUnit, WeightUnit, TemperatureUnit")]
    public string MeasurementType { get; set; } = string.Empty;

    public string? TargetUnit { get; set; }
}
