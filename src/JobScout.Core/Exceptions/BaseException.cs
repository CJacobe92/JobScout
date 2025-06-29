using System;

namespace JobScout.Core.Exceptions;

public class BaseException(string message) : Exception(message) { }
