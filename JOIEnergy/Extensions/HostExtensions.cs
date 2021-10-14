using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JOIEnergy.Application.Interfaces;
using JOIEnergy.Infrastructure.EF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace JOIEnergy.Extensions
{
    public static class HostExtensions
    {
        public static void MigrateDbContext<TContext>(this IWebHost host) where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                MigrationDbContext<TContext>(scope);
            }
        }

        public static void MigrateDbContext<TContext>(this IHost host) where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                MigrationDbContext<TContext>(scope);
            }
        }

        private static void MigrationDbContext<TContext>(IServiceScope scope) where TContext : DbContext
        {
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<TContext>();
                context.Database.Migrate();

                var initializer = services.GetRequiredService<IDatabaseInitializer>();
                initializer.Seed().Wait();
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred seeding the DB.");
            }
        }
    }
}
