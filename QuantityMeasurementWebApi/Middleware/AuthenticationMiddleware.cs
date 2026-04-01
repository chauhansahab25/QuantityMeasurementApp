using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using QuantityMeasurementBusinessLayer.Services;

namespace QuantityMeasurementWebApi.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory _scopeFactory;

    public JwtMiddleware(RequestDelegate next, IConfiguration configuration, IServiceScopeFactory scopeFactory)
    {
        _next = next;
        _configuration = configuration;
        _scopeFactory = scopeFactory;
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
        {
            await AttachUserToContext(context, token);
        }

        await _next(context);
    }

    private async Task AttachUserToContext(HttpContext context, string token)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var securityService = scope.ServiceProvider.GetRequiredService<ISecurityService>();
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

            // Validate token
            var principal = securityService.ValidateToken(token);
            
            if (principal == null)
            {
                return;
            }

            // Get session ID from token
            var sessionIdClaim = principal.FindFirst("session_id");
            if (sessionIdClaim == null)
            {
                return;
            }

            // Validate session
            var user = await userService.GetUserFromSessionAsync(sessionIdClaim.Value);
            if (user == null)
            {
                return;
            }

            // Attach user to context
            context.Items["User"] = user;
            context.User = principal;
        }
        catch (Exception ex)
        {
            // Log error but don't block the request
            using var scope = _scopeFactory.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<JwtMiddleware>>();
            logger.LogWarning(ex, "Error during JWT validation");
        }
    }
}

public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RateLimitingMiddleware> _logger;
    private readonly Dictionary<string, (int Count, DateTime ResetTime)> _requestCounts = new();
    private readonly object _lock = new object();

    public RateLimitingMiddleware(RequestDelegate next, ILogger<RateLimitingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var clientId = GetClientId(context);
        var limit = GetRequestLimit(context);
        var window = GetTimeWindow(context);

        lock (_lock)
        {
            if (!_requestCounts.ContainsKey(clientId))
            {
                _requestCounts[clientId] = (0, DateTime.UtcNow.Add(window));
            }

            var (count, resetTime) = _requestCounts[clientId];

            if (DateTime.UtcNow > resetTime)
            {
                _requestCounts[clientId] = (1, DateTime.UtcNow.Add(window));
            }
            else if (count >= limit)
            {
                _logger.LogWarning("Rate limit exceeded for client {ClientId}", clientId);
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.Response.Headers["Retry-After"] = ((int)(resetTime - DateTime.UtcNow).TotalSeconds).ToString();
                return;
            }
            else
            {
                _requestCounts[clientId] = (count + 1, resetTime);
            }
        }

        await _next(context);
    }

    private string GetClientId(HttpContext context)
    {
        // Use user ID if authenticated, otherwise IP address
        if (context.Items["User"] != null)
        {
            var user = (QuantityMeasurementModelLayer.Entities.User)context.Items["User"];
            return $"user:{user.Id}";
        }

        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        return $"ip:{ip}";
    }

    private int GetRequestLimit(HttpContext context)
    {
        // Different limits for different endpoints
        var path = context.Request.Path.Value?.ToLower() ?? "";
        
        return path switch
        {
            var p when p.Contains("/auth/") => 10, // Auth endpoints: 10 requests per window
            var p when p.Contains("/api/") => 100,  // API endpoints: 100 requests per window
            _ => 50 // Default: 50 requests per window
        };
    }

    private TimeSpan GetTimeWindow(HttpContext context)
    {
        var path = context.Request.Path.Value?.ToLower() ?? "";
        
        return path switch
        {
            var p when p.Contains("/auth/") => TimeSpan.FromMinutes(15), // Auth: 15 minutes
            var p when p.Contains("/api/") => TimeSpan.FromMinutes(5),   // API: 5 minutes
            _ => TimeSpan.FromMinutes(10) // Default: 10 minutes
        };
    }
}

public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;

    public SecurityHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // Add security headers
        context.Response.Headers["X-Content-Type-Options"] = "nosniff";
        context.Response.Headers["X-Frame-Options"] = "DENY";
        context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
        context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
        context.Response.Headers["Content-Security-Policy"] = "default-src 'self'; script-src 'self' 'unsafe-inline' 'unsafe-eval'; style-src 'self' 'unsafe-inline'; img-src 'self' data: https:; font-src 'self' data:; connect-src 'self'; frame-ancestors 'none';";
        
        // Remove server information
        context.Response.Headers.Remove("Server");

        await _next(context);
    }
}
