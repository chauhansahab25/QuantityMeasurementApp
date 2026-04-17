using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AuthenticationService.DTOs;
using AuthenticationService.Interfaces;
using AuthenticationService.Services;

namespace AuthenticationService.Controllers;

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

            var user = await _userService.CreateUserAsync(request);
            
            if (user == null)
            {
                return Conflict(new { error = "User with this email already exists" });
            }

            var sessionId = Guid.NewGuid().ToString();
            var jwtToken = _securityService.GenerateJwtToken(user, sessionId);

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

            await _userService.LogUserActivityAsync(user.Id, "REGISTER", ipAddress, userAgent);

            _logger.LogInformation("User registered successfully: {Email}", user.Email);

            return Ok(new LoginResponse
            {
                Token = jwtToken,
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = user.Role,
                    CreatedAt = user.CreatedAt
                },
                SessionId = sessionId
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user registration");
            return StatusCode(500, new { error = "Internal server error during registration" });
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

            var user = await _userService.AuthenticateAsync(request.Email, request.Password);
            
            if (user == null)
            {
                return Unauthorized(new { error = "Invalid email or password" });
            }

            var sessionId = Guid.NewGuid().ToString();
            var jwtToken = _securityService.GenerateJwtToken(user, sessionId);

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

            await _userService.LogUserActivityAsync(user.Id, "LOGIN", ipAddress, userAgent);

            _logger.LogInformation("User logged in successfully: {Email}", user.Email);

            return Ok(new LoginResponse
            {
                Token = jwtToken,
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = user.Role,
                    CreatedAt = user.CreatedAt
                },
                SessionId = sessionId
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user login");
            return StatusCode(500, new { error = "Internal server error during login" });
        }
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        try
        {
            var userId = GetCurrentUserId();
            var sessionId = User.FindFirst("sessionId")?.Value;

            if (!string.IsNullOrEmpty(sessionId))
            {
                await _userService.InvalidateSessionAsync(sessionId);
            }

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

            await _userService.LogUserActivityAsync(userId, "LOGOUT", ipAddress, userAgent);

            _logger.LogInformation("User logged out: {UserId}", userId);

            return Ok(new { message = "Logout successful" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user logout");
            return StatusCode(500, new { error = "Internal server error during logout" });
        }
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { error = "Invalid refresh token data", details = ModelState });
            }

            var principal = _securityService.GetPrincipalFromExpiredToken(request.Token);
            if (principal == null)
            {
                return Unauthorized(new { error = "Invalid token" });
            }

            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized(new { error = "Invalid token" });
            }

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return Unauthorized(new { error = "User not found" });
            }

            var sessionId = Guid.NewGuid().ToString();
            var newJwtToken = _securityService.GenerateJwtToken(user, sessionId);

            await _userService.InvalidateSessionAsync(request.SessionId);

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

            await _userService.LogUserActivityAsync(userId, "TOKEN_REFRESH", ipAddress, userAgent);

            _logger.LogInformation("Token refreshed for user: {UserId}", userId);

            return Ok(new LoginResponse
            {
                Token = newJwtToken,
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = user.Role,
                    CreatedAt = user.CreatedAt
                },
                SessionId = sessionId
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during token refresh");
            return StatusCode(500, new { error = "Internal server error during token refresh" });
        }
    }

    [HttpGet("validate")]
    [Authorize]
    public IActionResult ValidateToken()
    {
        try
        {
            var userId = GetCurrentUserId();
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new
            {
                Valid = true,
                UserId = userId,
                Email = email,
                Role = role
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during token validation");
            return StatusCode(500, new { error = "Internal server error during token validation" });
        }
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
}
