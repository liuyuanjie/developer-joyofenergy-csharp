using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Shouldly;
using Xunit;

namespace JOIEnergy.FunctionalTests.Scenarios
{
    public class PricePlanComparatorScenarios : ScenarioBase
    {
        [Fact]
        public async Task GivenExistingExistingSmartMeterIdShouldReturnAllPricePlans()
        {
            //Arrange
            var smartMeterId = "smart-meter-0";

            //Action
            var response = await SendAsync(client => client.GetAsync(string.Format(Get.CompareAll, smartMeterId)));

            //Assert
            var result = await RetrieveResultAsync<Dictionary<string, decimal>>(response);
            this.ShouldSatisfyAllConditions(
                () => result.Count.ShouldBeGreaterThan(0),
                () => response.StatusCode.ShouldBe((HttpStatusCode)StatusCodes.Status200OK)
            );
        }

        private static class Get
        {
            public static string CompareAll = "price-plans/compare-all/{0}";
            public static string Recommend = "price-plans/compare-all/{0}?limit={1}";
        }
    }
}
