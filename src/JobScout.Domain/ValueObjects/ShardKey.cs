using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScout.Domain.ValueObjects
{
    public class ShardKey: ValueObject
    {
        public string Value { get; private set; }

        public ShardKey(string value)
        {
            if(string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Shard key cannot be null or empty.", nameof(value));
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
