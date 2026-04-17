using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using QuantityMeasurementBusinessLayer.Services;
using QuantityMeasurementModelLayer.DTOs;

namespace QuantityMeasurementWebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ISecurityService _securityService;
    private readonly ILogger<AuthController> _logger;
    private readonly IConfiguration _configuration;

    public AuthController(
        IUserService userService,
        ISecurityService securityService,
        ILogger<AuthController> logger,
        IConfiguration configuration)
    {
        _userService = userService;
        _securityService = securityService;
        _logger = logger;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { error = "Invalid registration data", details = ModelState });
            }

            var result = await _userService.CreateUserAsync(request);
            
            if (result.AlreadyExists)
            {
                return Conflict(new { error = "User with this email already exists" });
            }

            if (result.ErrorMessage != null)
            {
                _logger.LogError("User creation failed with error: {Error}", result.ErrorMessage);
                return StatusCode(500, new { error = "Registration failed due to server error", details = result.ErrorMessage });
            }

            var user = result.User!;
            var sessionId = Guid.NewGuid().ToString();
            var jwtToken = _securityService.GenerateJwtToken(user, sessionId);

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var userAgent = Request.Headers["User-Agent"].ToString();
            
            await _userService.CreateUserSessionAsync(user, sessionId, ipAddress, userAgent);

            return Ok(new AuthResponse
            {
                Token = jwtToken,
                RefreshToken = _securityService.GenerateRefreshToken(),
                User = new UserInfo
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    CreatedAt = user.CreatedAt,
                    LastLoginAt = user.LastLoginAt
                },
                ExpiresAt = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JwtSettings:ExpirationMinutes"] ?? "60"))
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user registration");
            return StatusCode(500, new { error = "Registration failed", details = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { error = "Invalid login data", details = ModelState });
            }

            var user = await _userService.AuthenticateUserAsync(request.Email, request.Password);
            
            if (user == null)
            {
                return Unauthorized(new { error = "Invalid email or password" });
            }

            var sessionId = Guid.NewGuid().ToString();
            var jwtToken = _securityService.GenerateJwtToken(user, sessionId);

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var userAgent = Request.Headers["User-Agent"].ToString();
            
            await _userService.CreateUserSessionAsync(user, sessionId, ipAddress, userAgent);

            return Ok(new AuthResponse
            {
                Token = jwtToken,
                RefreshToken = _securityService.GenerateRefreshToken(),
                User = new UserInfo
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    CreatedAt = user.CreatedAt,
                    LastLoginAt = user.LastLoginAt
                },
                ExpiresAt = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JwtSettings:ExpirationMinutes"] ?? "60"))
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user login");
            return StatusCode(500, new { error = "Login failed" });
        }
    }
}

public class RefreshTokenRequest
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}

public class SessionInfo
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public string IpAddress { get; set; } = string.Empty;
    public string? UserAgent { get; set; }
    public bool IsCurrent { get; set; }
}
