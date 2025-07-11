using System;

namespace Shared.Exceptions;

public class AppException(string message, int statusCode = 500, string? code = null, object? metadata = null) : Exception(message)
{
    public int StatusCode { get; } = statusCode;
    public string? Code { get; } = code;
    public object? Metadata { get; } = metadata;
}
