using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JOIEnergy.FunctionalTests
{
    public class TestStartup : Startup
    {
        public IConfiguration Configuration { get; }

        public TestStartup(IConfiguration configuration) : base(configuration)
        {
            Configuration = configuration;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddApplicationPart(typeof(Startup).Assembly);

            base.ConfigureServices(services);
        }
    }
}
