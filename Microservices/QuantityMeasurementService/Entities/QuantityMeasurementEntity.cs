using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuantityMeasurementModelLayer.Entities;

[Table("QuantityMeasurements")]
public class QuantityMeasurementEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "float")]
    public double FirstValue { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstUnit { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "float")]
    public double SecondValue { get; set; }

    [Required]
    [MaxLength(50)]
    public string SecondUnit { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Operation { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "float")]
    public double Result { get; set; }

    [Required]
    [MaxLength(20)]
    public string MeasurementType { get; set; } = string.Empty;

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public bool IsError { get; set; } = false;

    public string? ErrorMessage { get; set; }

    [MaxLength(500)]
    public string? ResultString { get; set; }

    [Required]
    public int UserId { get; set; }

    public QuantityMeasurementEntity() { }

    public QuantityMeasurementEntity(
        double firstValue,
        string firstUnit,
        double secondValue,
        string secondUnit,
        string operation,
        double result,
        string measurementType)
    {
        FirstValue = firstValue;
        FirstUnit = firstUnit ?? string.Empty;
        SecondValue = secondValue;
        SecondUnit = secondUnit ?? string.Empty;
        Operation = operation ?? string.Empty;
        Result = result;
        MeasurementType = measurementType ?? string.Empty;
        CreatedAt = DateTime.UtcNow;
    }

    public QuantityMeasurementEntity(
        double firstValue,
        string firstUnit,
        double secondValue,
        string secondUnit,
        string operation,
        double result,
        string measurementType,
        bool isError,
        string? errorMessage = null,
        string? resultString = null)
    {
        FirstValue = firstValue;
        FirstUnit = firstUnit ?? string.Empty;
        SecondValue = secondValue;
        SecondUnit = secondUnit ?? string.Empty;
        Operation = operation ?? string.Empty;
        Result = result;
        MeasurementType = measurementType ?? string.Empty;
        CreatedAt = DateTime.UtcNow;
        IsError = isError;
        ErrorMessage = errorMessage;
        ResultString = resultString;
    }
}
