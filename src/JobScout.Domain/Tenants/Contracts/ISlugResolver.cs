using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScout.Domain.Tenants.Contracts
{
    public interface ISlugResolver
    {
        string ResolveFor(string companyName);
    }
}
