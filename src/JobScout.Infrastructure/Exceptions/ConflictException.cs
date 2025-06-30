using System;

namespace JobScout.Infrastructure.Exceptions;

public class ConflictException(string message) : BaseException(message) { }
