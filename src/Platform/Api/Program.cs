using Api;
using Infrastructure.Kafka;
using Infrastructure.Outbox;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

var connStr = builder.Configuration.GetSection("ConnectionString")["AppDb"];

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var redisConnStr = config.GetSection("Redis")["ConnectionString"];
    return ConnectionMultiplexer.Connect(redisConnStr!);
});

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(connStr));

builder.Services.AddScoped<IKafkaProducer, KafkaProducer>();
builder.Services.AddHostedService<OutboxProcessor>();

var app = builder.Build();
app.MapTenantEndpoints();
app.UseResponseCompression();
app.Run();
