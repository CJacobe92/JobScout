using System;

namespace JobScout.Core.Exceptions;

public class ConflictException(string message) : BaseException(message) { }
