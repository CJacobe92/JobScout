using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScout.Domain.SeedWork
{
    public interface IUniquenessChecker<T>
    {
        Task<bool> IsUniqueAsync(T value);
    }
}
