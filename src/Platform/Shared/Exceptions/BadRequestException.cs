using System;

namespace Shared.Exceptions;

public class BadRequestException(string message, object? metadata = null) :
AppException(message, 400, "BAD_REQUEST", metadata)
{ }
