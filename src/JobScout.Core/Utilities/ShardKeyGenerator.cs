using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScout.Core.Utilities;

public static class ShardKeyGenerator
{
    public static string From(string companyName)
    {
        if (string.IsNullOrWhiteSpace(companyName))
            throw new ArgumentException("Company name cannot be null or empty.");

        // Example: Take first character and normalize
        var prefix = companyName.Trim().ToLowerInvariant()[0];

        // Hash example for more spread (or use CRC32, MurmurHash, etc.)
        var hash = Math.Abs(companyName.GetHashCode()) % 2; // assumes 3 shards: 0, 1, 2

        return $"shard0{hash}";
    }
}

