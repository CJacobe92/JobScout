using Api;
using Domain.Repositories;
using FluentValidation;
using Infrastructure.Kafka;
using Infrastructure.Outbox;
using Infrastructure.Persistence;
using Infrastructure.Redis;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.SeedWork;
using StackExchange.Redis;
using Serilog;
using Application;

var builder = WebApplication.CreateBuilder(args);

var connStr = builder.Configuration.GetSection("ConnectionString")["AppDb"];

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Information()
    .CreateLogger();

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});

builder.Host.UseSerilog();
builder.Services.AddValidatorsFromAssemblies(
    [typeof(TenantApplicationAssemblyMarker).Assembly]
);

builder.Services.AddScoped<ITenantRepository, TenantRepository>();

builder.Services.Configure<RedisSettings>(
    builder.Configuration.GetSection("Redis"));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    typeof(TenantApplicationAssemblyMarker).Assembly
));

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<RedisSettings>>().Value;
    var connStr = $"{settings.Host}:{settings.Port}";
    return ConnectionMultiplexer.Connect(connStr);
});

builder.Services.AddSingleton<ICacheService, RedisCacheService>();


builder.Services.AddDbContext<AppDbContext>(options =>
options.UseNpgsql(connStr, npgsqlOptions =>
{
    npgsqlOptions.EnableRetryOnFailure(
        maxRetryCount: 5,
        maxRetryDelay: TimeSpan.FromSeconds(10),
        errorCodesToAdd: ["57P01", "40001"]
    );
}));

builder.Services.AddScoped<IKafkaProducer, KafkaProducer>();
builder.Services.AddHostedService<OutboxProcessor>();

var app = builder.Build();
app.MapTenantEndpoints();
app.UseResponseCompression();
app.Run();
