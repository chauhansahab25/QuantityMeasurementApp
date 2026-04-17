using AuthenticationService.DTOs;
using QuantityMeasurementModelLayer.Entities;

namespace AuthenticationService.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> CreateUserAsync(RegisterUserRequest request);
        Task<User?> AuthenticateUserAsync(string email, string password);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<DTOs.UserSession> CreateUserSessionAsync(User user, string jwtTokenId, string ipAddress, string? userAgent = null);
        Task<bool> RevokeSessionAsync(string jwtTokenId);
        Task<bool> ValidateSessionAsync(string jwtTokenId);
        Task<User?> GetUserFromSessionAsync(string jwtTokenId);
        Task<List<DTOs.UserSession>> GetUserActiveSessionsAsync(int userId);
        Task<bool> RevokeAllUserSessionsAsync(int userId);
        Task LogUserActivityAsync(int userId, string activity, string ipAddress, string userAgent);
        Task InvalidateSessionAsync(string sessionId);
        Task<User?> GetUserByIdAsync(int userId);
        Task<User?> AuthenticateAsync(string email, string password);
    }
}
