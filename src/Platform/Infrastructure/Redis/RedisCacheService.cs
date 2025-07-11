using System;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Shared.SeedWork;
using StackExchange.Redis;

namespace Infrastructure.Redis;

public class RedisCacheService : ICacheService
{
    private readonly RedisSettings _settings;
    private readonly IDatabase _database;

    private readonly IConnectionMultiplexer _redis;

    public RedisCacheService(IConnectionMultiplexer redis, IOptions<RedisSettings> options)
    {
        _settings = options.Value;
        _redis = redis;
        _database = redis.GetDatabase(_settings.DbIndex);
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        cancellationToken.ThrowIfCancellationRequested();
        var cachedValue = await _database.StringGetAsync(key);

        if (cachedValue.HasValue)
        {
            var result = JsonSerializer.Deserialize<T>(cachedValue!);

            await _database.KeyExpireAsync(key, TimeSpan.FromSeconds(_settings.DefaultExpirationSeconds));
            return result;
        }

        return default;
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpirationRelativeToNow = null, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var jsonValue = JsonSerializer.Serialize(value);
        var ttl = absoluteExpirationRelativeToNow ?? TimeSpan.FromSeconds(_settings.DefaultExpirationSeconds);
        await _database.StringSetAsync(key, jsonValue, ttl);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await _database.KeyDeleteAsync(key);
    }

    public async Task RemoveByPatternAsync(string pattern, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var server = _redis.GetServer($"{_settings.Host}:{_settings.Port}");
        var keys = server.Keys(pattern: pattern).ToArray();

        foreach (var key in keys)
        {
            await _database.KeyDeleteAsync(key);
        }
    }
}
