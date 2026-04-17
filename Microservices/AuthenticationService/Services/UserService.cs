using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuantityMeasurementModelLayer.Entities;
using AuthenticationService.Context;
using AuthenticationService.Interfaces;
using AuthenticationService.DTOs;

namespace AuthenticationService.Services;

public class UserService : Interfaces.IUserService
{
    private readonly QuantityMeasurementDbContext _context;
    private readonly ISecurityService _securityService;
    private readonly ILogger<UserService> _logger;

    public UserService(QuantityMeasurementDbContext context, ISecurityService securityService, ILogger<UserService> logger)
    {
        _context = context;
        _securityService = securityService;
        _logger = logger;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        try
        {
            var user = await _context.Users
                .Include(u => u.UserSessions)
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower() && u.IsActive);
            
            _logger.LogDebug("User lookup by email {Email}: {Found}", email, user != null);
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user by email {Email}", email);
            return null;
        }
    }

    public async Task<User?> CreateUserAsync(RegisterUserRequest request)
    {
        try
        {
            // Check if user already exists
            var existingUser = await GetUserByEmailAsync(request.Email);
            if (existingUser != null)
            {
                _logger.LogWarning("User creation failed - email already exists: {Email}", request.Email);
                return null;
            }

            // Hash password
            var passwordHash = _securityService.HashPassword(request.Password);

            // Create new user
            var newUser = new User
            {
                Email = request.Email.ToLower(),
                PasswordHash = passwordHash,
                FirstName = request.FirstName,
                LastName = request.LastName,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created new user {UserId} for email {Email}", newUser.Id, newUser.Email);
            return newUser;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user for email {Email}", request.Email);
            return null;
        }
    }

    public async Task<User?> AuthenticateUserAsync(string email, string password)
    {
        try
        {
            var user = await GetUserByEmailAsync(email);
            
            if (user == null)
            {
                _logger.LogWarning("Authentication failed - user not found: {Email}", email);
                return null;
            }

            // Verify password
            if (!_securityService.VerifyPassword(password, user.PasswordHash))
            {
                _logger.LogWarning("Authentication failed - invalid password for email: {Email}", email);
                return null;
            }

            // Update last login
            user.LastLoginAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            _logger.LogInformation("User authenticated successfully: {Email}", email);
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during authentication for email {Email}", email);
            return null;
        }
    }

    public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
            
            if (user == null)
            {
                _logger.LogWarning("Password change failed - user not found: {UserId}", userId);
                return false;
            }

            // Verify current password
            if (!_securityService.VerifyPassword(currentPassword, user.PasswordHash))
            {
                _logger.LogWarning("Password change failed - invalid current password for user: {UserId}", userId);
                return false;
            }

            // Hash new password
            user.PasswordHash = _securityService.HashPassword(newPassword);
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Password changed successfully for user: {UserId}", userId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing password for user {UserId}", userId);
            return false;
        }
    }

    public async Task<DTOs.UserSession> CreateUserSessionAsync(User user, string jwtTokenId, string ipAddress, string? userAgent = null)
    {
        try
        {
            var session = new QuantityMeasurementModelLayer.Entities.UserSession
            {
                UserId = user.Id,
                JwtTokenId = jwtTokenId,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddHours(1), // Default session expiry
                IpAddress = ipAddress,
                UserAgent = userAgent,
                IsActive = true
            };

            _context.UserSessions.Add(session);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created session {SessionId} for user {UserId}", session.Id, user.Id);
            
            // Convert Entity.UserSession to DTOs.UserSession
            return new DTOs.UserSession
            {
                Id = session.Id,
                UserId = session.UserId,
                SessionId = session.JwtTokenId,
                IpAddress = session.IpAddress ?? string.Empty,
                UserAgent = session.UserAgent ?? string.Empty,
                CreatedAt = session.CreatedAt,
                ExpiresAt = session.ExpiresAt ?? DateTime.UtcNow.AddHours(1)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating session for user {UserId}", user.Id);
            throw;
        }
    }

    public async Task<bool> RevokeSessionAsync(string jwtTokenId)
    {
        try
        {
            var session = await _context.UserSessions
                .FirstOrDefaultAsync(s => s.JwtTokenId == jwtTokenId && s.IsActive);

            if (session != null)
            {
                session.IsActive = false;
                session.RevokedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                _logger.LogInformation("Revoked session {SessionId} with JWT ID {JwtTokenId}", session.Id, jwtTokenId);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error revoking session with JWT ID {JwtTokenId}", jwtTokenId);
            return false;
        }
    }

    public async Task<bool> ValidateSessionAsync(string jwtTokenId)
    {
        try
        {
            var session = await _context.UserSessions
                .FirstOrDefaultAsync(s => s.JwtTokenId == jwtTokenId && s.IsActive);

            if (session != null)
            {
                // Check if session has expired
                if (session.ExpiresAt.HasValue && session.ExpiresAt.Value < DateTime.UtcNow)
                {
                    session.IsActive = false;
                    session.RevokedAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                    
                    _logger.LogWarning("Session {SessionId} expired and was revoked", session.Id);
                    return false;
                }

                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating session with JWT ID {JwtTokenId}", jwtTokenId);
            return false;
        }
    }

    public async Task<User?> GetUserFromSessionAsync(string jwtTokenId)
    {
        try
        {
            var session = await _context.UserSessions
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.JwtTokenId == jwtTokenId && s.IsActive);

            if (session != null && await ValidateSessionAsync(jwtTokenId))
            {
                return session.User;
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user from session with JWT ID {JwtTokenId}", jwtTokenId);
            return null;
        }
    }

    public async Task<List<DTOs.UserSession>> GetUserActiveSessionsAsync(int userId)
    {
        try
        {
            var sessions = await _context.UserSessions
                .Where(s => s.UserId == userId && s.IsActive)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();

            // Remove expired sessions
            var expiredSessions = sessions.Where(s => s.ExpiresAt.HasValue && s.ExpiresAt.Value < DateTime.UtcNow).ToList();
            foreach (var expiredSession in expiredSessions)
            {
                expiredSession.IsActive = false;
                expiredSession.RevokedAt = DateTime.UtcNow;
            }

            if (expiredSessions.Any())
            {
                await _context.SaveChangesAsync();
            }

            // Convert Entity.UserSession list to DTOs.UserSession list
            return sessions
                .Where(s => !expiredSessions.Contains(s))
                .Select(s => new DTOs.UserSession
                {
                    Id = s.Id,
                    UserId = s.UserId,
                    SessionId = s.JwtTokenId,
                    IpAddress = s.IpAddress ?? string.Empty,
                    UserAgent = s.UserAgent ?? string.Empty,
                    CreatedAt = s.CreatedAt,
                    ExpiresAt = s.ExpiresAt ?? DateTime.UtcNow.AddHours(1)
                })
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting active sessions for user {UserId}", userId);
            return new List<DTOs.UserSession>();
        }
    }

    public async Task<bool> RevokeAllUserSessionsAsync(int userId)
    {
        try
        {
            var sessions = await _context.UserSessions
                .Where(s => s.UserId == userId && s.IsActive)
                .ToListAsync();

            foreach (var session in sessions)
            {
                session.IsActive = false;
                session.RevokedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Revoked {Count} sessions for user {UserId}", sessions.Count, userId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error revoking all sessions for user {UserId}", userId);
            return false;
        }
    }

    public async Task<User?> AuthenticateAsync(string email, string password)
    {
        return await AuthenticateUserAsync(email, password);
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        try
        {
            return await _context.Users.FindAsync(userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user by id {UserId}", userId);
            return null;
        }
    }

    public async Task LogUserActivityAsync(int userId, string activity, string ipAddress, string userAgent)
    {
        try
        {
            var userActivity = new DTOs.UserActivity
            {
                UserId = userId,
                Activity = activity,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                CreatedAt = DateTime.UtcNow
            };
            // Note: need an entity for UserActivity if we want to save it to DB. For now, logging.
            _logger.LogInformation("Activity Log - User: {UserId}, Activity: {Activity}, IP: {IpAddress}, Agent: {UserAgent}", 
                userId, activity, ipAddress, userAgent);
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error logging activity for user {UserId}", userId);
        }
    }

    public async Task InvalidateSessionAsync(string sessionId)
    {
        await RevokeSessionAsync(sessionId);
    }
}
