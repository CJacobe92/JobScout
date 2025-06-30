using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace JobScout.Infrastructure.Database.Context;

public class DesignTimeShardDbContextFactory : IDesignTimeDbContextFactory<ShardDbContext>
{
    public ShardDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: false)
           .AddEnvironmentVariables()
           .Build();

        var shard00ConnStr = config.GetSection("ShardConnections")["shard00"];
        var builder = new DbContextOptionsBuilder<ShardDbContext>();
        builder.UseNpgsql(shard00ConnStr);
        return new ShardDbContext(builder.Options);
    }
}
