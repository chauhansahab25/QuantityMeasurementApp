using Microsoft.AspNetCore.Mvc;
using QuantityMeasurementWebApi.DTOs;
using QuantityMeasurementBusinessLayer.Services;
using QuantityMeasurementBusinessLayer.Interfaces;
using QuantityMeasurementModelLayer.DTO;

namespace QuantityMeasurementWebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class QuantityMeasurementController : ControllerBase
{
    private readonly IQuantityMeasurementService _quantityService;
    private readonly QuantityMeasurementBusinessLayer.Services.IRedisCacheService _cacheService;
    private readonly ILogger<QuantityMeasurementController> _logger;

    public QuantityMeasurementController(
        IQuantityMeasurementService quantityService,
        QuantityMeasurementBusinessLayer.Services.IRedisCacheService cacheService,
        ILogger<QuantityMeasurementController> logger)
    {
        _quantityService = quantityService;
        _cacheService = cacheService;
        _logger = logger;
    }

    [HttpPost("compare")]
    public async Task<ActionResult<QuantityMeasurementOperationResultDto>> Compare([FromBody] QuantityMeasurementRequestDto request)
    {
        try
        {
            var cacheKey = $"compare_{request.FirstValue}_{request.FirstUnit}_{request.SecondValue}_{request.SecondUnit}";
            
            var cachedResult = await _cacheService.GetAsync<QuantityMeasurementOperationResultDto>(cacheKey);
            if (cachedResult != null)
            {
                _logger.LogInformation($"Returning cached result for comparison: {cacheKey}");
                return Ok(cachedResult);
            }

            var q1 = new QuantityDTO(request.FirstValue, request.FirstUnit);
            var q2 = new QuantityDTO(request.SecondValue, request.SecondUnit);
            
            var result = _quantityService.Compare(q1, q2);

            var response = new QuantityMeasurementOperationResultDto
            {
                Result = result ? 1 : 0,
                ResultString = result ? "Equal" : "Not Equal",
                IsError = false,
                Operation = "COMPARE",
                MeasurementType = request.MeasurementType
            };

            await _cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(30));
            
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Compare operation");
            var errorResponse = new QuantityMeasurementOperationResultDto
            {
                IsError = true,
                ErrorMessage = ex.Message,
                Operation = "COMPARE",
                MeasurementType = request.MeasurementType
            };
            return BadRequest(errorResponse);
        }
    }

    [HttpPost("convert")]
    public async Task<ActionResult<QuantityMeasurementOperationResultDto>> Convert(
        [FromBody] QuantityMeasurementRequestDto request, 
        [FromQuery] string targetUnit)
    {
        try
        {
            if (string.IsNullOrEmpty(targetUnit))
            {
                return BadRequest(new QuantityMeasurementOperationResultDto
                {
                    IsError = true,
                    ErrorMessage = "Target unit is required for conversion",
                    Operation = "CONVERT",
                    MeasurementType = request.MeasurementType
                });
            }

            var cacheKey = $"convert_{request.FirstValue}_{request.FirstUnit}_{targetUnit}_{request.MeasurementType}";
            
            var cachedResult = await _cacheService.GetAsync<QuantityMeasurementOperationResultDto>(cacheKey);
            if (cachedResult != null)
            {
                _logger.LogInformation($"Returning cached result for conversion: {cacheKey}");
                return Ok(cachedResult);
            }

            var input = new QuantityDTO(request.FirstValue, request.FirstUnit);
            var result = _quantityService.Convert(input, targetUnit);

            var response = new QuantityMeasurementOperationResultDto
            {
                Result = result.Value,
                ResultString = $"{request.FirstValue} {request.FirstUnit} = {result.Value} {targetUnit}",
                IsError = false,
                Operation = "CONVERT",
                MeasurementType = request.MeasurementType
            };

            await _cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(30));
            
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Convert operation");
            var errorResponse = new QuantityMeasurementOperationResultDto
            {
                IsError = true,
                ErrorMessage = ex.Message,
                Operation = "CONVERT",
                MeasurementType = request.MeasurementType
            };
            return BadRequest(errorResponse);
        }
    }

    [HttpPost("add")]
    public async Task<ActionResult<QuantityMeasurementOperationResultDto>> Add([FromBody] QuantityMeasurementRequestDto request)
    {
        try
        {
            var cacheKey = $"add_{request.FirstValue}_{request.FirstUnit}_{request.SecondValue}_{request.SecondUnit}";
            
            var cachedResult = await _cacheService.GetAsync<QuantityMeasurementOperationResultDto>(cacheKey);
            if (cachedResult != null)
            {
                _logger.LogInformation($"Returning cached result for addition: {cacheKey}");
                return Ok(cachedResult);
            }

            var q1 = new QuantityDTO(request.FirstValue, request.FirstUnit);
            var q2 = new QuantityDTO(request.SecondValue, request.SecondUnit);
            
            var result = _quantityService.Add(q1, q2);

            var response = new QuantityMeasurementOperationResultDto
            {
                Result = result.Value,
                ResultString = $"{request.FirstValue} {request.FirstUnit} + {request.SecondValue} {request.SecondUnit} = {result.Value} {result.Unit}",
                IsError = false,
                Operation = "ADD",
                MeasurementType = request.MeasurementType
            };

            await _cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(30));
            
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Add operation");
            var errorResponse = new QuantityMeasurementOperationResultDto
            {
                IsError = true,
                ErrorMessage = ex.Message,
                Operation = "ADD",
                MeasurementType = request.MeasurementType
            };
            return BadRequest(errorResponse);
        }
    }

    [HttpPost("subtract")]
    public async Task<ActionResult<QuantityMeasurementOperationResultDto>> Subtract([FromBody] QuantityMeasurementRequestDto request)
    {
        try
        {
            var cacheKey = $"subtract_{request.FirstValue}_{request.FirstUnit}_{request.SecondValue}_{request.SecondUnit}";
            
            var cachedResult = await _cacheService.GetAsync<QuantityMeasurementOperationResultDto>(cacheKey);
            if (cachedResult != null)
            {
                _logger.LogInformation($"Returning cached result for subtraction: {cacheKey}");
                return Ok(cachedResult);
            }

            var q1 = new QuantityDTO(request.FirstValue, request.FirstUnit);
            var q2 = new QuantityDTO(request.SecondValue, request.SecondUnit);
            
            var result = _quantityService.Subtract(q1, q2);

            var response = new QuantityMeasurementOperationResultDto
            {
                Result = result.Value,
                ResultString = $"{request.FirstValue} {request.FirstUnit} - {request.SecondValue} {request.SecondUnit} = {result.Value} {result.Unit}",
                IsError = false,
                Operation = "SUBTRACT",
                MeasurementType = request.MeasurementType
            };

            await _cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(30));
            
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Subtract operation");
            var errorResponse = new QuantityMeasurementOperationResultDto
            {
                IsError = true,
                ErrorMessage = ex.Message,
                Operation = "SUBTRACT",
                MeasurementType = request.MeasurementType
            };
            return BadRequest(errorResponse);
        }
    }

    [HttpPost("divide")]
    public async Task<ActionResult<QuantityMeasurementOperationResultDto>> Divide([FromBody] QuantityMeasurementRequestDto request)
    {
        try
        {
            if (request.SecondValue == 0)
            {
                return BadRequest(new QuantityMeasurementOperationResultDto
                {
                    IsError = true,
                    ErrorMessage = "Cannot divide by zero",
                    Operation = "DIVIDE",
                    MeasurementType = request.MeasurementType
                });
            }

            var cacheKey = $"divide_{request.FirstValue}_{request.FirstUnit}_{request.SecondValue}_{request.SecondUnit}";
            
            var cachedResult = await _cacheService.GetAsync<QuantityMeasurementOperationResultDto>(cacheKey);
            if (cachedResult != null)
            {
                _logger.LogInformation($"Returning cached result for division: {cacheKey}");
                return Ok(cachedResult);
            }

            var q1 = new QuantityDTO(request.FirstValue, request.FirstUnit);
            var q2 = new QuantityDTO(request.SecondValue, request.SecondUnit);
            
            var result = _quantityService.Divide(q1, q2);

            var response = new QuantityMeasurementOperationResultDto
            {
                Result = result,
                ResultString = $"{request.FirstValue} {request.FirstUnit} ÷ {request.SecondValue} {request.SecondUnit} = {result}",
                IsError = false,
                Operation = "DIVIDE",
                MeasurementType = request.MeasurementType
            };

            await _cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(30));
            
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Divide operation");
            var errorResponse = new QuantityMeasurementOperationResultDto
            {
                IsError = true,
                ErrorMessage = ex.Message,
                Operation = "DIVIDE",
                MeasurementType = request.MeasurementType
            };
            return BadRequest(errorResponse);
        }
    }
}
