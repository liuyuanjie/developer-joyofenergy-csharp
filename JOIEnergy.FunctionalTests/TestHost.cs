using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using JOIEnergy.Application.Interfaces;
using JOIEnergy.Extensions;
using JOIEnergy.Infrastructure.EF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
