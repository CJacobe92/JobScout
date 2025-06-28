using JobScout.AppService;
using JobScout.Core;
using JobScout.Domain.Contracts;
using JobScout.Infrastructure.Database.Context;
using JobScout.Infrastructure.Database.Repositories;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using JobScout.Infrastructure.Database.Entities;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);

var defaultConnStr = builder.Configuration.GetConnectionString("DefaultConnection");
var shard00ConnStr = builder.Configuration.GetSection("ShardConnections")["shard00"];

builder.Services.AddDbContext<CoreDbContext>(options =>
{
    options.UseNpgsql(defaultConnStr);
});

builder.Services.AddDbContext<ShardDbContext>(options =>
{
    options.UseNpgsql(shard00ConnStr);
});

builder.Services.AddIdentity<TenantUser, IdentityRole<Guid>>() // <– use the EF Identity entity
    .AddEntityFrameworkStores<ShardDbContext>();

builder.Services.AddScoped<ITenantRepository, TenantRepository>();
builder.Services.AddAutoMapper(typeof(_ForAppServiceAssemblyLoadOnly).Assembly);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(_ForCoreAssemblyLoadOnly).Assembly));

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
