using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using QuantityMeasurementModelLayer.Entities;

namespace AuthenticationService.Services;

public interface ISecurityService
{
    string GenerateJwtToken(User user, string sessionId);
    ClaimsPrincipal? ValidateToken(string token);
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
    string EncryptData(string data);
    string DecryptData(string encryptedData);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}

public class SecurityService : ISecurityService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<SecurityService> _logger;
    private readonly string _jwtKey;
    private readonly string _jwtIssuer;
    private readonly string _jwtAudience;
    private readonly int _jwtExpirationMinutes;
    private readonly string _encryptionKey;

    public SecurityService(IConfiguration configuration, ILogger<SecurityService> logger)
    {
        _configuration = configuration;
        _logger = logger;
        
        _jwtKey = _configuration["JwtSettings:Key"] ?? throw new ArgumentNullException("JWT Key not configured");
        _jwtIssuer = _configuration["JwtSettings:Issuer"] ?? "QuantityMeasurementAPI";
        _jwtAudience = _configuration["JwtSettings:Audience"] ?? "QuantityMeasurementUsers";
        _jwtExpirationMinutes = int.Parse(_configuration["JwtSettings:ExpirationMinutes"] ?? "60");
        _encryptionKey = _configuration["EncryptionSettings:Key"] ?? throw new ArgumentNullException("Encryption Key not configured");
    }

    public string GenerateJwtToken(User user, string sessionId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtKey);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.GivenName, user.FirstName),
            new Claim(ClaimTypes.Surname, user.LastName),
            new Claim("session_id", sessionId),
            new Claim("jti", Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, "User")
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtExpirationMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _jwtIssuer,
            Audience = _jwtAudience
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        _logger.LogInformation("JWT token generated for user {UserId} with session {SessionId}", user.Id, sessionId);
        return tokenString;
    }

    public ClaimsPrincipal? ValidateToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _jwtIssuer,
                ValidateAudience = true,
                ValidAudience = _jwtAudience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            
            if (validatedToken is JwtSecurityToken jwtToken)
            {
                // Additional validation for algorithm
                if (jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    _logger.LogDebug("Token validation successful for user");
                    return principal;
                }
            }

            _logger.LogWarning("Token validation failed - invalid algorithm");
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Token validation failed");
            return null;
        }
    }

    public string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var saltedPassword = password + _encryptionKey;
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
        return Convert.ToBase64String(hashedBytes);
    }

    public bool VerifyPassword(string password, string hash)
    {
        var computedHash = HashPassword(password);
        return computedHash.Equals(hash, StringComparison.OrdinalIgnoreCase);
    }

    public string EncryptData(string data)
    {
        try
        {
            using var aes = Aes.Create();
            var key = Encoding.UTF8.GetBytes(_encryptionKey);
            aes.Key = key;
            aes.GenerateIV();

            using var encryptor = aes.CreateEncryptor();
            using var msEncrypt = new MemoryStream();
            using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(data);
            }

            var encrypted = msEncrypt.ToArray();
            var result = new byte[aes.IV.Length + encrypted.Length];
            Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);
            Buffer.BlockCopy(encrypted, 0, result, aes.IV.Length, encrypted.Length);

            return Convert.ToBase64String(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Data encryption failed");
            throw;
        }
    }

    public string DecryptData(string encryptedData)
    {
        try
        {
            var fullCipher = Convert.FromBase64String(encryptedData);
            var iv = new byte[16];
            var cipher = new byte[fullCipher.Length - iv.Length];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, cipher.Length);

            using var aes = Aes.Create();
            var key = Encoding.UTF8.GetBytes(_encryptionKey);
            aes.Key = key;
            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor();
            using var msDecrypt = new MemoryStream(cipher);
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);
            
            return srDecrypt.ReadToEnd();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Data decryption failed");
            throw;
        }
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        try
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey)),
                ValidateLifetime = false // Don't care about expiration
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            
            if (securityToken is not JwtSecurityToken jwtSecurityToken || 
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return principal;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get principal from expired token");
            return null;
        }
    }
}
