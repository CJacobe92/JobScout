using JobScout.App.Extensions;
using JobScout.Infrastructure.Extensions;
using JobScout.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAppExtensions(builder.Configuration);
builder.Services.AddCoreServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
