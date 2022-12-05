using Microsoft.Extensions.Caching.Memory;

namespace TabpediaFin.Infrastructure.Caching;

public class LocalCacheService : ICacheService
{
    private readonly ILogger<LocalCacheService> _logger;
    private readonly IMemoryCache _cache;

    public LocalCacheService(IMemoryCache cache, ILogger<LocalCacheService> logger)
        => (_cache, _logger) = (cache, logger);

    public T? Get<T>(string key)
        => _cache.Get<T>(key);

    public Task<T?> GetAsync<T>(string key, CancellationToken token = default)
        => Task.FromResult(Get<T>(key));

    //public void Refresh(string key)
    //    => _cache.TryGetValue(key, out _);

    //public Task RefreshAsync(string key, CancellationToken token = default)
    //{
    //    Refresh(key);
    //    return Task.CompletedTask;
    //}

    public void Remove(string key)
        => _cache.Remove(key);

    public Task RemoveAsync(string key, CancellationToken token = default)
    {
        Remove(key);
        return Task.CompletedTask;
    }

    public void Set<T>(string key, T value, TimeSpan? slidingExpiration = null)
    {
        if (slidingExpiration is null)
        {
            // TODO: add to appsettings?
            slidingExpiration = TimeSpan.FromDays(1); // Default expiration time of 1 day //10 minutes.
        }

        _cache.Set(key, value, new MemoryCacheEntryOptions { SlidingExpiration = slidingExpiration });
        _logger.LogDebug($"Added to Cache : {key}", key);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? slidingExpiration = null, CancellationToken token = default)
    {
        Set(key, value, slidingExpiration);
        return Task.CompletedTask;
    }
}


public interface ICacheService
{
    T? Get<T>(string key);

    Task<T?> GetAsync<T>(string key, CancellationToken token = default);

    //void Refresh(string key);

    //Task RefreshAsync(string key, CancellationToken token = default);

    void Remove(string key);

    Task RemoveAsync(string key, CancellationToken token = default);

    void Set<T>(string key, T value, TimeSpan? slidingExpiration = null);

    Task SetAsync<T>(string key, T value, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default);
}
