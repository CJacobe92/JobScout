using System;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Shared.SeedWork;
using StackExchange.Redis;

namespace Infrastructure.Redis;

public class RedisCacheService : ICacheService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _database;
    private readonly IConfiguration _config;
    private readonly TimeSpan _defaultExpiration = TimeSpan.FromSeconds(30);

    public RedisCacheService(IConnectionMultiplexer redis, IConfiguration config)
    {
        _redis = redis ?? throw new ArgumentNullException(nameof(redis));
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _database = _redis.GetDatabase();
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        cancellationToken.ThrowIfCancellationRequested();
        var cachedValue = await _database.StringGetAsync(key);

        if (cachedValue.HasValue)
        {
            return JsonSerializer.Deserialize<T>(cachedValue!);
        }

        return default;
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpirationRelativeToNow = null, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var jsonValue = JsonSerializer.Serialize(value);
        await _database.StringSetAsync(key, jsonValue, absoluteExpirationRelativeToNow ?? _defaultExpiration);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await _database.KeyDeleteAsync(key);
    }

    public async Task RemoveByPatternAsync(string pattern, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var redisHost = _config["Redis:Host"] ?? "redis";
        var redisPort = _config["Redis:Port"] ?? "6379";

        var server = _redis.GetServer($"{redisHost}:{redisPort}");

        var keys = server.Keys(pattern: pattern).ToArray();

        foreach (var key in keys)
        {
            await _database.KeyDeleteAsync(key);
        }
    }
}

