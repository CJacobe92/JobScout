using System;

namespace Shared.Exceptions;

public class NotFoundException(string message, object? metadata = null) :
AppException(message, 404, "NOT_FOUND", metadata)
{ }
