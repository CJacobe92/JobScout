using System;

namespace JobScout.Core.Exceptions;

public class NotFoundException(string message) : BaseException(message) { }
