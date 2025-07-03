using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JobScout.Domain.SeedWork
{
    public abstract class Entity<TId> : EntityBase
    {
        public TId? Id { get; init; }
    }
}
