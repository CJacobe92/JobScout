using System;

namespace JobScout.Core.Exceptions;

public class BadRequestException(string message) : BaseException(message) { }

