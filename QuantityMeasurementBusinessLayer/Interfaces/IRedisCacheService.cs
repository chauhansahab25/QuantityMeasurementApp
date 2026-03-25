public interface IRedisCacheService
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpirationRelativeToNow = null);
    Task RemoveAsync(string key);
    Task ClearAsync();
}