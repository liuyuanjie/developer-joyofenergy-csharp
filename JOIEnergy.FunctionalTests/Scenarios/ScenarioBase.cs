using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using JOIEnergy.Infrastructure.EF;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace JOIEnergy.FunctionalTests.Scenarios
{
    public abstract class ScenarioBase : IDisposable
    {
        protected TestServer Server { get; set; }
        protected JOIEnergyDbContext DbContext { get; set; }

        protected ScenarioBase()
        {
            Server = TestHost.CreateServer();
            DbContext = Server.Host.Services.GetRequiredService<JOIEnergyDbContext>();
            //DbContext.Database.EnsureDeleted();
            //DbContext.Database.EnsureCreated();
        }

        public void Dispose()
        {
            DbContext.Database.EnsureDeleted();

            Server.Dispose();
            Server = null;
            DbContext.Dispose();
            DbContext = null;
        }

        protected async Task<TOut> SendAsync<TOut>(Func<HttpClient, Task<TOut>> func)
        {
            using (var client = Server.CreateClient())
            {
                return await func.Invoke(client);
            }
        }

        protected async Task<T> RetrieveResultAsync<T>(HttpResponseMessage message)
        {
            return await message.Content.ReadFromJsonAsync<T>();
        }
    }
}
