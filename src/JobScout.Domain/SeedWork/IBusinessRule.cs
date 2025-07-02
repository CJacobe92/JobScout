using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScout.Domain.SeedWork
{
    public interface IBusinessRule
    {
        string Message { get; }
        Task<bool> IsBrokenAsync();
    }

}
