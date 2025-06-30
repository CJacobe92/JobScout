using JobScout.API.Extensions;
using JobScout.API.Middlewares;
using JobScout.Core.Extensions;
using JobScout.Infrastructure.Database.Context;
using JobScout.Infrastructure.Database.Entities;
using JobScout.Infrastructure.Extensions;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// 🚀 Services
builder.Services.AddAuthorization();
builder.Services.AddResponseCompression(o => o.EnableForHttps = true);
builder.Services.AddSingleton<MetricsLabelingMiddleware>();
builder.WebHost.ConfigureKestrel(opts =>
{
    opts.ListenAnyIP(8081, listenOpts => listenOpts.UseHttps());
    opts.ListenAnyIP(5135);
});


builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddCoreServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);

var useInMemory = builder.Configuration.GetValue<bool>("UseInMemoryDatabase");

var app = builder.Build();


if (!app.Environment.IsDevelopment()) // or IsProduction()
{
    app.UseHttpsRedirection();
}
// 🌐 Middleware
app.UseResponseCompression();
app.UseRouting();
app.UseHttpMetrics();
app.UseAuthorization();
app.UseMiddleware<MetricsLabelingMiddleware>();

// 🔧 Endpoints
app.MapMetrics();
app.MapGraphQL(); // Presuming you wired this up inside AddApiServices
app.MapGet("/ping", () => Results.Ok("pong"));

// 🧪 Optional data seeding
if (useInMemory)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<CoreDbContext>();

    if (!db.Tenants.Any())
    {
        for (int i = 0; i < 100; i++)
        {
            db.Tenants.Add(new TenantEntity
            {
                Id = Guid.NewGuid(),
                CompanyName = $"Seeded Company {i}",
                ShardKey = $"shard0{i % 2}",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        await db.SaveChangesAsync();
        Console.WriteLine("✅ Seeded 100 mock tenants");
    }
}

// 🧠 Diagnostics
Console.WriteLine($"ENV: {builder.Environment.EnvironmentName}");
Console.WriteLine($"GC Server: {System.Runtime.GCSettings.IsServerGC}");
ThreadPool.GetMinThreads(out var workers, out var iocp);
Console.WriteLine($"Min Worker Threads: {workers}, IOCP Threads: {iocp}");
Console.WriteLine($"🧪 UseInMemoryDatabase: {useInMemory}");

await app.RunAsync();
