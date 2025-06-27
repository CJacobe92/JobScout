using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScout.Domain.ValueObjects
{
    public class CompanyName: ValueObject
    {
        public string Value { get; private set; }

        public CompanyName(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) 
            {
                throw new ArgumentException("Company name cannot be null or empty.", nameof(value));
            }

            Value = value.Trim();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value.ToLowerInvariant();
        }

        public override string ToString() => Value;

    }
}
