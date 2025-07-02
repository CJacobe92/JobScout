using JobScout.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScout.Domain.Tenants.Rules
{
    public class CompanyNameMustBeUniqueRule(string companyName, IUniquenessChecker<string> checker) : IBusinessRule
    {
        private readonly string _companyName = companyName;
        private readonly IUniquenessChecker<string> _checker = checker;

        public string Message => $"Company name '{_companyName}' must be unique";

        public async Task<bool> IsBrokenAsync() =>
            !await _checker.IsUniqueAsync(_companyName);
    }

}
