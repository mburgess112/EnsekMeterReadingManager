using EnsekMeterReadingManager.Models;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using EnsekMeterReadingManager.Csv;

namespace EnsekMeterReadingManager
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<CsvMeterReadingExtractor>();
            services.AddSingleton<MeterReadingDtoConverter>();
            services.AddScoped<MeterReadingRepository>();

            services.AddDbContext<EnsekDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("EnsekConnection")));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EnsekMeterReadingManager", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EnsekMeterReadingManager v1"));
                UpdateDevDatabase(app);
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        // TODO: would be neater as an extension method
        private static void UpdateDevDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<EnsekDbContext>();
            context.Database.Migrate();
            //TODO: since this is dev-only anyway, consider wiring in an import of the test account CSV
        }
    }
}
