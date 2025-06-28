// using System;

// namespace JobScout.Infrastructure.Database.Connection;

// public class TenantConnectionStringProvider
// {
//   public string GetConnectionString()
//   {
//     var shardKey = _tenantContext.Tenant?.ShardKey;

//     var connections = _configuration.GetSection("ShardConnections").Get<Dictionary<string, string>>();

//     if (string.IsNullOrWhiteSpace(shardKey) || !connections.ContainsKey(shardKey))
//       throw new Exception($"Shard key '{shardKey}' is invalid or unregistered.");

//     return connections[shardKey];
//   }

// }
