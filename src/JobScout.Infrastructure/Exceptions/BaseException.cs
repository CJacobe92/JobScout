using System;

namespace JobScout.Infrastructure.Exceptions;

public class BaseException(string message) : Exception(message) { }
