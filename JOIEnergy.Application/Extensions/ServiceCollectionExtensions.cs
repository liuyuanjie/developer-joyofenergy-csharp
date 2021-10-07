using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;
using JOIEnergy.Application.Behaviors;
using MediatR;

namespace JOIEnergy.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
            services.AddMediatR(Assembly.GetExecutingAssembly());
        }
    }
}
