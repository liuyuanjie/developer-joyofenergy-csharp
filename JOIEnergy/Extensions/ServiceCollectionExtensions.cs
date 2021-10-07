using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JOIEnergy.Domain.Interfaces;
using JOIEnergy.Infrastructure.EF;
using Microsoft.Extensions.DependencyInjection;

namespace JOIEnergy.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, JOIEnergyDbContext>();
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
            return services;
        }
    }
}
