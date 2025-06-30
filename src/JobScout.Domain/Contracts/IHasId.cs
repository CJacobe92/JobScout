using System;

namespace JobScout.Domain.Contracts;

public interface IHasId
{
    Guid? Id { get; set; }
}

