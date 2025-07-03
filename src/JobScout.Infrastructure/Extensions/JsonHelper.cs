using System.Text.Json;

namespace JobScout.Infrastructure.Extensions;

public static class JsonHelper
{
    private static readonly JsonSerializerOptions _defaultOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static string Serialize(object value, Type type) =>
        JsonSerializer.Serialize(value, type, _defaultOptions);

    public static T Deserialize<T>(string json) =>
        JsonSerializer.Deserialize<T>(json, _defaultOptions)!;

    public static object Deserialize(string json, Type type) =>
        JsonSerializer.Deserialize(json, type, _defaultOptions)!;

    public static JsonSerializerOptions Options => _defaultOptions;
}
