using Microsoft.AspNetCore.Mvc;
using QuantityMeasurementWebApi.DTOs;
using QuantityMeasurementRepositoryLayer.Interfaces;
using QuantityMeasurementModelLayer.Entities;

namespace QuantityMeasurementWebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class HistoryController : ControllerBase
{
    private readonly QuantityMeasurementRepositoryLayer.Interfaces.IQuantityMeasurementRepository _repository;
    private readonly ILogger<HistoryController> _logger;

    public HistoryController(
        QuantityMeasurementRepositoryLayer.Interfaces.IQuantityMeasurementRepository repository,
        ILogger<HistoryController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// Get all measurement history
    /// </summary>
    /// <returns>List of all measurements</returns>
    [HttpGet]
    public async Task<ActionResult<List<QuantityMeasurementResponseDto>>> GetAll()
    {
        try
        {
            var measurements = await _repository.GetAllAsync();
            var response = measurements.Select(MapToResponseDto).ToList();
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all measurements");
            return StatusCode(500, new { error = "Internal server error", message = ex.Message });
        }
    }

    /// <summary>
    /// Get measurements by operation type
    /// </summary>
    /// <param name="operation">Operation type (ADD, SUBTRACT, MULTIPLY, DIVIDE, COMPARE, CONVERT)</param>
    /// <returns>List of measurements for the specified operation</returns>
    [HttpGet("operation/{operation}")]
    public async Task<ActionResult<List<QuantityMeasurementResponseDto>>> GetByOperation(string operation)
    {
        try
        {
            var measurements = await _repository.GetByOperationAsync(operation);
            var response = measurements.Select(MapToResponseDto).ToList();
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving measurements by operation: {operation}");
            return StatusCode(500, new { error = "Internal server error", message = ex.Message });
        }
    }

    /// <summary>
    /// Get measurements by measurement type
    /// </summary>
    /// <param name="measurementType">Measurement type (LengthUnit, VolumeUnit, WeightUnit, TemperatureUnit)</param>
    /// <returns>List of measurements for the specified type</returns>
    [HttpGet("type/{measurementType}")]
    public async Task<ActionResult<List<QuantityMeasurementResponseDto>>> GetByMeasurementType(string measurementType)
    {
        try
        {
            var measurements = await _repository.GetByMeasurementTypeAsync(measurementType);
            var response = measurements.Select(MapToResponseDto).ToList();
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving measurements by measurement type: {measurementType}");
            return StatusCode(500, new { error = "Internal server error", message = ex.Message });
        }
    }

    /// <summary>
    /// Get measurements by date range
    /// </summary>
    /// <param name="startDate">Start date (ISO 8601 format)</param>
    /// <param name="endDate">End date (ISO 8601 format)</param>
    /// <returns>List of measurements within the date range</returns>
    [HttpGet("daterange")]
    public async Task<ActionResult<List<QuantityMeasurementResponseDto>>> GetByDateRange(
        [FromQuery] DateTime startDate, 
        [FromQuery] DateTime endDate)
    {
        try
        {
            if (startDate > endDate)
            {
                return BadRequest(new { error = "Bad request", message = "Start date must be before end date" });
            }

            var measurements = await _repository.GetByDateRangeAsync(startDate, endDate);
            var response = measurements.Select(MapToResponseDto).ToList();
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving measurements by date range: {startDate} to {endDate}");
            return StatusCode(500, new { error = "Internal server error", message = ex.Message });
        }
    }

    /// <summary>
    /// Get errored measurements
    /// </summary>
    /// <returns>List of measurements that resulted in errors</returns>
    [HttpGet("errors")]
    public async Task<ActionResult<List<QuantityMeasurementResponseDto>>> GetErroredMeasurements()
    {
        try
        {
            var measurements = await _repository.GetErroredMeasurementsAsync();
            var response = measurements.Select(MapToResponseDto).ToList();
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving errored measurements");
            return StatusCode(500, new { error = "Internal server error", message = ex.Message });
        }
    }

    /// <summary>
    /// Get the last saved measurement
    /// </summary>
    /// <returns>Last saved measurement</returns>
    [HttpGet("last")]
    public async Task<ActionResult<QuantityMeasurementResponseDto>> GetLastSaved()
    {
        try
        {
            var measurement = await _repository.GetLastSavedOperationAsync();
            if (measurement == null)
            {
                return NotFound(new { error = "Not found", message = "No measurements found" });
            }

            return Ok(MapToResponseDto(measurement));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving last saved measurement");
            return StatusCode(500, new { error = "Internal server error", message = ex.Message });
        }
    }

    /// <summary>
    /// Get count of measurements by operation type
    /// </summary>
    /// <param name="operation">Operation type</param>
    /// <returns>Count of measurements for the specified operation</returns>
    [HttpGet("count/{operation}")]
    public async Task<ActionResult<int>> GetCountByOperation(string operation)
    {
        try
        {
            var count = await _repository.GetCountByOperationAsync(operation);
            return Ok(count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting count by operation: {operation}");
            return StatusCode(500, new { error = "Internal server error", message = ex.Message });
        }
    }

    /// <summary>
    /// Get total count of all measurements
    /// </summary>
    /// <returns>Total count of measurements</returns>
    [HttpGet("count")]
    public async Task<ActionResult<int>> GetTotalCount()
    {
        try
        {
            var count = await _repository.GetTotalCountAsync();
            return Ok(count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting total count of measurements");
            return StatusCode(500, new { error = "Internal server error", message = ex.Message });
        }
    }

    /// <summary>
    /// Test database connectivity
    /// </summary>
    /// <returns>Database connectivity status</returns>
    [HttpGet("health")]
    public async Task<ActionResult<object>> GetHealthStatus()
    {
        try
        {
            var isConnected = await _repository.TestConnectionAsync();
            var totalCount = isConnected ? await _repository.GetTotalCountAsync() : 0;

            return Ok(new
            {
                status = isConnected ? "Healthy" : "Unhealthy",
                isConnected = isConnected,
                totalRecords = totalCount,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking database health");
            return StatusCode(500, new
            {
                status = "Unhealthy",
                isConnected = false,
                error = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    /// <summary>
    /// Delete all measurements (admin operation)
    /// </summary>
    /// <returns>Deletion result</returns>
    [HttpDelete]
    public async Task<ActionResult<object>> DeleteAll()
    {
        try
        {
            await _repository.DeleteAllAsync();
            _logger.LogWarning("All measurements deleted via API");
            return Ok(new { message = "All measurements deleted successfully", timestamp = DateTime.UtcNow });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting all measurements");
            return StatusCode(500, new { error = "Internal server error", message = ex.Message });
        }
    }

    private static QuantityMeasurementResponseDto MapToResponseDto(QuantityMeasurementEntity entity)
    {
        return new QuantityMeasurementResponseDto
        {
            Id = entity.Id,
            FirstValue = entity.FirstValue,
            FirstUnit = entity.FirstUnit,
            SecondValue = entity.SecondValue,
            SecondUnit = entity.SecondUnit,
            Operation = entity.Operation,
            Result = entity.Result,
            MeasurementType = entity.MeasurementType,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            IsError = entity.IsError,
            ErrorMessage = entity.ErrorMessage,
            ResultString = entity.ResultString
        };
    }
}
