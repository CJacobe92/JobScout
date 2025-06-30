using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace JobScout.Infrastructure.Database.Context;

public class DesignTimeCoreDbContextFactory : IDesignTimeDbContextFactory<CoreDbContext>
{
    public CoreDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false)
        .AddEnvironmentVariables()
        .Build();

        var builder = new DbContextOptionsBuilder<CoreDbContext>();
        builder.UseNpgsql(config.GetConnectionString("DefaultConnection"));
        return new CoreDbContext(builder.Options);
    }
}
