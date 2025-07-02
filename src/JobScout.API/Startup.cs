using JobScout.API.Configuration;
using JobScout.Application.Configuration;
using JobScout.Infrastructure;
using JobScout.Infrastructure.Registrations;

namespace JobScout.API
{
    public class Startup(IWebHostEnvironment env)
    {
        private readonly IConfiguration _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json")
                .AddUserSecrets<Startup>()
                .Build();

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var appDbConnStr = _configuration.GetSection("ConnectionStrings")["AppDb"];
            var tenantDbConnStr = _configuration.GetSection("ConnectionStrings")["TenantDb"];

            APIModule.Register(services);
            ApplicationModule.Register(services);
            InfrastructureModule.Register(services, appDbConnStr!, tenantDbConnStr!);
        }
    }
}