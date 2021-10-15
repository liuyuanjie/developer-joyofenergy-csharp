using System.IO;
using System.Reflection;
using JOIEnergy.Extensions;
using JOIEnergy.Infrastructure.EF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace JOIEnergy.FunctionalTests
{
    public class TestHost
    {
        public static TestServer CreateServer()
        {
            var path = Assembly.GetAssembly(typeof(TestHost)).Location;

            var hostBuilder = new WebHostBuilder()
                .UseContentRoot(Path.GetDirectoryName(path))
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                })
                .ConfigureAppConfiguration(cb =>
                {
                    cb.AddJsonFile("appsettings.json").AddEnvironmentVariables();
                })
                .UseStartup<TestStartup>();

            var testServer = new TestServer(hostBuilder);
            testServer.Host.MigrateDbContext<JOIEnergyDbContext>();

            return testServer;
        }
    }
}
