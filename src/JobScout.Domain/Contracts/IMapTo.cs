using System;

namespace JobScout.Domain.Contracts;

public interface IMapTo<out T>
{
    T Map();
}
