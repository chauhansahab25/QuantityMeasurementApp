using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using QuantityMeasurementService.DTOs;
using QuantityMeasurementBusinessLayer.Services;
using QuantityMeasurementService.Interfaces;
using QuantityMeasurementModelLayer.DTO;
using System.Security.Claims;

namespace QuantityMeasurementService.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
[Produces("application/json")]
public class QuantityMeasurementController : ControllerBase
{
    private readonly IQuantityMeasurementService _quantityService;
    private readonly IRedisCacheService _cacheService;
    private readonly ILogger<QuantityMeasurementController> _logger;

    public QuantityMeasurementController(
        IQuantityMeasurementService quantityService,
        IRedisCacheService cacheService,
        ILogger<QuantityMeasurementController> logger)
    {
        _quantityService = quantityService;
        _cacheService = cacheService;
        _logger = logger;
    }

    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            throw new UnauthorizedAccessException("User ID not found in token");
        }
        return userId;
    }

    [HttpPost("compare")]
    public async Task<ActionResult<QuantityMeasurementOperationResultDto>> Compare([FromBody] QuantityMeasurementRequestDto request)
    {
        try
        {
            var userId = GetCurrentUserId();
            var cacheKey = $"compare_{request.FirstValue}_{request.FirstUnit}_{request.SecondValue}_{request.SecondUnit}_{userId}";
            
            var cachedResult = await _cacheService.GetAsync<QuantityMeasurementOperationResultDto>(cacheKey);
            if (cachedResult != null)
            {
                _logger.LogInformation("Returning cached result for comparison operation");
                return Ok(cachedResult);
            }

            var q1 = new QuantityDTO(request.FirstValue, request.FirstUnit);
            var q2 = new QuantityDTO(request.SecondValue, request.SecondUnit);
            
            var result = _quantityService.Compare(q1, q2, userId);
            
            var response = new QuantityMeasurementOperationResultDto
            {
                Result = result ? 1 : 0,
                Operation = "COMPARE",
                FirstQuantity = q1,
                SecondQuantity = q2,
                Timestamp = DateTime.UtcNow
            };

            await _cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(30));
            
            _logger.LogInformation("Comparison operation completed successfully for user {UserId}", userId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during comparison operation");
            return StatusCode(500, new { error = "Internal server error during comparison" });
        }
    }

    [HttpPost("add")]
    public async Task<ActionResult<QuantityMeasurementOperationResultDto>> Add([FromBody] QuantityMeasurementRequestDto request)
    {
        try
        {
            var userId = GetCurrentUserId();
            var cacheKey = $"add_{request.FirstValue}_{request.FirstUnit}_{request.SecondValue}_{request.SecondUnit}_{userId}";
            
            var cachedResult = await _cacheService.GetAsync<QuantityMeasurementOperationResultDto>(cacheKey);
            if (cachedResult != null)
            {
                _logger.LogInformation("Returning cached result for addition operation");
                return Ok(cachedResult);
            }

            var q1 = new QuantityDTO(request.FirstValue, request.FirstUnit);
            var q2 = new QuantityDTO(request.SecondValue, request.SecondUnit);
            
            var result = _quantityService.Add(q1, q2, userId);
            
            var response = new QuantityMeasurementOperationResultDto
            {
                Result = result.Value,
                ResultUnit = result.Unit,
                Operation = "ADD",
                FirstQuantity = q1,
                SecondQuantity = q2,
                Timestamp = DateTime.UtcNow
            };

            await _cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(30));
            
            _logger.LogInformation("Addition operation completed successfully for user {UserId}", userId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during addition operation");
            return StatusCode(500, new { error = "Internal server error during addition" });
        }
    }

    [HttpPost("subtract")]
    public async Task<ActionResult<QuantityMeasurementOperationResultDto>> Subtract([FromBody] QuantityMeasurementRequestDto request)
    {
        try
        {
            var userId = GetCurrentUserId();
            var cacheKey = $"subtract_{request.FirstValue}_{request.FirstUnit}_{request.SecondValue}_{request.SecondUnit}_{userId}";
            
            var cachedResult = await _cacheService.GetAsync<QuantityMeasurementOperationResultDto>(cacheKey);
            if (cachedResult != null)
            {
                _logger.LogInformation("Returning cached result for subtraction operation");
                return Ok(cachedResult);
            }

            var q1 = new QuantityDTO(request.FirstValue, request.FirstUnit);
            var q2 = new QuantityDTO(request.SecondValue, request.SecondUnit);
            
            var result = _quantityService.Subtract(q1, q2, userId);
            
            var response = new QuantityMeasurementOperationResultDto
            {
                Result = result.Value,
                ResultUnit = result.Unit,
                Operation = "SUBTRACT",
                FirstQuantity = q1,
                SecondQuantity = q2,
                Timestamp = DateTime.UtcNow
            };

            await _cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(30));
            
            _logger.LogInformation("Subtraction operation completed successfully for user {UserId}", userId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during subtraction operation");
            return StatusCode(500, new { error = "Internal server error during subtraction" });
        }
    }

    [HttpPost("divide")]
    public async Task<ActionResult<QuantityMeasurementOperationResultDto>> Divide([FromBody] QuantityMeasurementRequestDto request)
    {
        try
        {
            var userId = GetCurrentUserId();
            var cacheKey = $"divide_{request.FirstValue}_{request.FirstUnit}_{request.SecondValue}_{request.SecondUnit}_{userId}";
            
            var cachedResult = await _cacheService.GetAsync<QuantityMeasurementOperationResultDto>(cacheKey);
            if (cachedResult != null)
            {
                _logger.LogInformation("Returning cached result for division operation");
                return Ok(cachedResult);
            }

            var q1 = new QuantityDTO(request.FirstValue, request.FirstUnit);
            var q2 = new QuantityDTO(request.SecondValue, request.SecondUnit);
            
            var result = _quantityService.Divide(q1, q2, userId);
            
            var response = new QuantityMeasurementOperationResultDto
            {
                Result = result,
                Operation = "DIVIDE",
                FirstQuantity = q1,
                SecondQuantity = q2,
                Timestamp = DateTime.UtcNow
            };

            await _cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(30));
            
            _logger.LogInformation("Division operation completed successfully for user {UserId}", userId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during division operation");
            return StatusCode(500, new { error = "Internal server error during division" });
        }
    }

    [HttpPost("convert")]
    public async Task<ActionResult<QuantityMeasurementOperationResultDto>> Convert([FromBody] QuantityMeasurementConvertRequestDto request)
    {
        try
        {
            var userId = GetCurrentUserId();
            var cacheKey = $"convert_{request.Value}_{request.FromUnit}_{request.ToUnit}_{userId}";
            
            var cachedResult = await _cacheService.GetAsync<QuantityMeasurementOperationResultDto>(cacheKey);
            if (cachedResult != null)
            {
                _logger.LogInformation("Returning cached result for conversion operation");
                return Ok(cachedResult);
            }

            var input = new QuantityDTO(request.Value, request.FromUnit);
            var result = _quantityService.Convert(input, request.ToUnit, userId);
            
            var response = new QuantityMeasurementOperationResultDto
            {
                Result = result.Value,
                ResultUnit = result.Unit,
                Operation = "CONVERT",
                FirstQuantity = input,
                Timestamp = DateTime.UtcNow
            };

            await _cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(30));
            
            _logger.LogInformation("Conversion operation completed successfully for user {UserId}", userId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during conversion operation");
            return StatusCode(500, new { error = "Internal server error during conversion" });
        }
    }

    [HttpGet("history")]
    public ActionResult<IEnumerable<QuantityMeasurementHistoryDto>> GetHistory()
    {
        try
        {
            var userId = GetCurrentUserId();
            var history = _quantityService.GetByUserId(userId);
            
            var result = history.Select(h => new QuantityMeasurementHistoryDto
            {
                Id = h.Id,
                FirstValue = h.FirstValue,
                FirstUnit = h.FirstUnit,
                SecondValue = h.SecondValue,
                SecondUnit = h.SecondUnit,
                Operation = h.Operation,
                Result = h.Result,
                MeasurementType = h.MeasurementType,
                CreatedAt = h.CreatedAt
            }).ToList();

            _logger.LogInformation("Retrieved {Count} history records for user {UserId}", result.Count, userId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving measurement history");
            return StatusCode(500, new { error = "Internal server error retrieving history" });
        }
    }

    [HttpGet("history/{operation}")]
    public ActionResult<IEnumerable<QuantityMeasurementHistoryDto>> GetHistoryByOperation(string operation)
    {
        try
        {
            var userId = GetCurrentUserId();
            var history = _quantityService.GetByUserIdOperation(userId, operation.ToUpper());
            
            var result = history.Select(h => new QuantityMeasurementHistoryDto
            {
                Id = h.Id,
                FirstValue = h.FirstValue,
                FirstUnit = h.FirstUnit,
                SecondValue = h.SecondValue,
                SecondUnit = h.SecondUnit,
                Operation = h.Operation,
                Result = h.Result,
                MeasurementType = h.MeasurementType,
                CreatedAt = h.CreatedAt
            }).ToList();

            _logger.LogInformation("Retrieved {Count} history records for operation {Operation} for user {UserId}", result.Count, operation, userId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving measurement history by operation");
            return StatusCode(500, new { error = "Internal server error retrieving history by operation" });
        }
    }

    [HttpDelete("history")]
    public ActionResult<IActionResult> ClearHistory()
    {
        try
        {
            var userId = GetCurrentUserId();
            _quantityService.DeleteByUserId(userId);
            
            _logger.LogInformation("Cleared measurement history for user {UserId}", userId);
            return Ok(new { message = "History cleared successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clearing measurement history");
            return StatusCode(500, new { error = "Internal server error clearing history" });
        }
    }
}
