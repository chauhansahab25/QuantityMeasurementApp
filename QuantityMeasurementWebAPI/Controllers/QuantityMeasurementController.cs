using Microsoft.AspNetCore.Mvc;
using QuantityMeasurementApp.Model.DTOs;
using QuantityMeasurementApp.Model.Exceptions;
using QuantityMeasurementApp.Bussiness.Services;

namespace QuantityMeasurementWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuantityMeasurementController : ControllerBase
    {
        private readonly IQuantityMeasurementService _service;

        public QuantityMeasurementController(IQuantityMeasurementService service)
        {
            _service = service;
        }

        [HttpPost("compare")]
        public IActionResult PerformComparison([FromBody] ComparisonRequest request)
        {
            try
            {
                var result = _service.Compare(request.Quantity1, request.Quantity2);
                return Ok(new { equal = result.Value == 1, message = "Comparison successful" });
            }
            catch (QuantityMeasurementException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("convert")]
        public IActionResult PerformConversion([FromBody] ConversionRequest request)
        {
            try
            {
                var result = _service.Convert(request.Quantity, request.TargetUnit);
                return Ok(new { value = result.Value, unit = result.Unit, message = "Conversion successful" });
            }
            catch (QuantityMeasurementException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("add")]
        public IActionResult PerformAddition([FromBody] ArithmeticRequest request)
        {
            try
            {
                var result = _service.Add(request.Quantity1, request.Quantity2);
                return Ok(new { value = result.Value, unit = result.Unit, message = "Addition successful" });
            }
            catch (QuantityMeasurementException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("subtract")]
        public IActionResult PerformSubtraction([FromBody] ArithmeticRequest request)
        {
            try
            {
                var result = _service.Subtract(request.Quantity1, request.Quantity2);
                return Ok(new { value = result.Value, unit = result.Unit, message = "Subtraction successful" });
            }
            catch (QuantityMeasurementException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("divide")]
        public IActionResult PerformDivision([FromBody] ArithmeticRequest request)
        {
            try
            {
                var result = _service.Divide(request.Quantity1, request.Quantity2);
                return Ok(new { value = result.Value, message = "Division successful (dimensionless)" });
            }
            catch (QuantityMeasurementException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        public class ComparisonRequest
        {
            public QuantityDTO Quantity1 { get; set; }
            public QuantityDTO Quantity2 { get; set; }
        }

        public class ConversionRequest
        {
            public QuantityDTO Quantity { get; set; }
            public string TargetUnit { get; set; }
        }

        public class ArithmeticRequest
        {
            public QuantityDTO Quantity1 { get; set; }
            public QuantityDTO Quantity2 { get; set; }
        }
    }
}