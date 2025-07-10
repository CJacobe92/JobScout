using Api;
using Application.Tenants.Commands.CreateTenant;
using Application.Tenants.Validators.CreateTenant;
using Application.Tenants.Validators.GetAllTenants;
using Domain.Repositories;
using FluentValidation;
using Infrastructure.Kafka;
using Infrastructure.Outbox;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

var connStr = builder.Configuration.GetSection("ConnectionString")["AppDb"];

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});

builder.Services.AddValidatorsFromAssemblyContaining<CreateTenantDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<GetAllTenantsDtoValidator>();
builder.Services.AddScoped<CreateTenantHandler>();
builder.Services.AddScoped<ITenantRepository, TenantRepository>();


builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var redisConnStr = config.GetSection("Redis")["ConnectionString"];
    return ConnectionMultiplexer.Connect(redisConnStr!);
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
