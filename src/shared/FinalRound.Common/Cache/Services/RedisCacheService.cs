using System.Text.Json;
using FinalRound.Common.Cache.Options;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace FinalRound.Common.Cache.Services;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);

    Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? absoluteExpirationRelativeToNow = null,
        CancellationToken cancellationToken = default);

    Task RemoveAsync(string key, CancellationToken cancellationToken = default);

    Task<T?> GetOrSetAsync<T>(
        string key,
        Func<CancellationToken, Task<T>> factory,
        TimeSpan? absoluteExpirationRelativeToNow = null,
        bool cacheNullValues = false,
        CancellationToken cancellationToken = default);

    Task<T?> GetOrSetAsync<T>(
        string key,
        Func<Task<T>> factory,
        TimeSpan? absoluteExpirationRelativeToNow = null,
        bool cacheNullValues = false,
        CancellationToken cancellationToken = default);

}

public sealed class RedisCacheService : ICacheService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly IDistributedCache _cache;
    private readonly RedisCacheOptions _options;

    public RedisCacheService(
        IDistributedCache cache,
        IOptions<RedisCacheOptions> options)
    {
        _cache = cache;
        _options = options.Value;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var json = await _cache.GetStringAsync(key, cancellationToken);

        if (string.IsNullOrWhiteSpace(json))
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(json, JsonOptions);
    }

    public async Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? absoluteExpirationRelativeToNow = null,
        CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(value, JsonOptions);

        var cacheEntryOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow =
                absoluteExpirationRelativeToNow ??
                TimeSpan.FromMinutes(_options.DefaultExpirationMinutes)
        };

        await _cache.SetStringAsync(key, json, cacheEntryOptions, cancellationToken);
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        => _cache.RemoveAsync(key, cancellationToken);

    public async Task<T?> GetOrSetAsync<T>(
        string key,
        Func<CancellationToken, Task<T>> factory,
        TimeSpan? absoluteExpirationRelativeToNow = null,
        bool cacheNullValues = false,
        CancellationToken cancellationToken = default)
    {
        var cachedValue = await GetAsync<T>(key, cancellationToken);

        if (!IsDefaultValue(cachedValue))
        {
            return cachedValue;
        }

        var value = await factory(cancellationToken);

        if (cacheNullValues || !IsDefaultValue(value))
        {
            await SetAsync(key, value, absoluteExpirationRelativeToNow, cancellationToken);
        }

        return value;
    }

    public Task<T?> GetOrSetAsync<T>(
        string key,
        Func<Task<T>> factory,
        TimeSpan? absoluteExpirationRelativeToNow = null,
        bool cacheNullValues = false,
        CancellationToken cancellationToken = default)
    {
        return GetOrSetAsync(
            key,
            _ => factory(),
            absoluteExpirationRelativeToNow,
            cacheNullValues,
            cancellationToken);
    }

    private static bool IsDefaultValue<T>(T? value)
        => EqualityComparer<T>.Default.Equals(value!, default!);
}