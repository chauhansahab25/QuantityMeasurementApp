using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

using QuantityMeasurementService.Interfaces;

namespace QuantityMeasurementService.Services;

public class RedisCacheService : IRedisCacheService
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<RedisCacheService> _logger;

    public RedisCacheService(IDistributedCache cache, ILogger<RedisCacheService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        try
        {
            var cachedData = await _cache.GetStringAsync(key);
            if (cachedData == null)
            {
                return default(T);
            }

            return JsonSerializer.Deserialize<T>(cachedData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting cached data for key: {key}");
            return default(T);
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpirationRelativeToNow = null)
    {
        try
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow ?? TimeSpan.FromMinutes(30)
            };

            var serializedData = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, serializedData, options);
            
            _logger.LogInformation($"Cached data for key: {key}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error caching data for key: {key}");
            throw;
        }
    }

    public async Task RemoveAsync(string key)
    {
        try
        {
            await _cache.RemoveAsync(key);
            _logger.LogInformation($"Removed cached data for key: {key}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error removing cached data for key: {key}");
            throw;
        }
    }

    public async Task ClearAsync()
    {
        try
        {
            // Note: This is a simplified implementation
            // In a real scenario, you might want to track keys and remove them individually
            // or use Redis-specific commands for clearing all data
            _logger.LogInformation("Cache clear requested - implement based on your Redis provider capabilities");
            
            // For now, this is a placeholder
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clearing cache");
            throw;
        }
    }
}
