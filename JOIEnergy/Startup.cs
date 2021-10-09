using JOIEnergy.Application;
using JOIEnergy.Application.Extensions;
using JOIEnergy.Application.Interfaces;
using JOIEnergy.Application.Services;
using JOIEnergy.Application.UsageCostPricePlan;
using JOIEnergy.Extensions;
using JOIEnergy.Infrastructure.EF;
using JOIEnergy.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace JOIEnergy
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JOIEnergy.Infrastructure.Share.DateTimeConverter());
                });
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IMeterReadingService, MeterReadingService>();
            services.AddTransient<IPricePlanService, PricePlanService>();
            services.AddTransient<IPricePlanComparatorService, PricePlanComparatorService>();
            services.AddTransient<IUsageCostPricePlanProvider, AverageUsageCostPricePlanProvider>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "JOI Energy", Version = "v1" });
            });

            services.AddDbContext<JOIEnergyDbContext>(config =>
            {
                config.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")).UseLazyLoadingProxies();
            });
            services.AddTransient<IDatabaseInitializer, DatabaseInitializer>();
            services.AddTransient<IConnection, MSSqlConnection>();
            services.AddRepositories();
            services.AddSingleton(provider => new ConnectionSettings(Configuration.GetConnectionString("DefaultConnection")));
            services.AddApplicationServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "JOI Energy Api v1"));
            }

            app.UseHttpsRedirection();
            app.UseErrorHandler();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
