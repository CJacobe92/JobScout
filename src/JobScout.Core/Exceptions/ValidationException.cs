using System;

namespace JobScout.Core.Exceptions;

public class ValidationException(string message) : BaseException(message) { }
