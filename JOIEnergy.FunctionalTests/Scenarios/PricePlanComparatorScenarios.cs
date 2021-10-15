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
        public async Task GivenValidSmartMeterIdShouldReturnAllPricePlans()
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

        [Fact]
        public async Task GivenInvalidSmartMeterIdShouldReturnAllPricePlans()
        {
            //Arrange
            var smartMeterId = "smart-meter-x";

            //Action
            var response = await SendAsync(client => client.GetAsync(string.Format(Get.CompareAll, smartMeterId)));

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GivenRecommendLimitIsNullShouldReturnAllRecommendedUsageCostPricePlans()
        {
            //Arrange
            var smartMeterId = "smart-meter-0";
            var maxNumberUsageCost = 3;

            //Action
            var response = await SendAsync(client => client.GetAsync(string.Format(Get.Recommend, smartMeterId)));

            //Assert
            var result = await RetrieveResultAsync<Dictionary<string, decimal>>(response);
            this.ShouldSatisfyAllConditions(
                () => result.Count.ShouldBe(maxNumberUsageCost),
                () => response.StatusCode.ShouldBe((HttpStatusCode)StatusCodes.Status200OK)
            );
        }

        [Fact]
        public async Task GivenRecommendLimitIsLessThanPricePansShouldReturnRecommendedLimitUsageCostPricePlans()
        {
            //Arrange
            var smartMeterId = "smart-meter-0";
            var limit = 2;

            //Action
            var response = await SendAsync(client => client.GetAsync(string.Format($"{Get.Recommend}?limit={limit}", smartMeterId)));

            //Assert
            var result = await RetrieveResultAsync<Dictionary<string, decimal>>(response);
            this.ShouldSatisfyAllConditions(
                () => result.Count.ShouldBe(limit),
                () => response.StatusCode.ShouldBe((HttpStatusCode)StatusCodes.Status200OK)
            );
        }

        [Fact]
        public async Task GivenRecommendLimitMoreThanPricePansShouldReturnRecommendedAllUsageCostPricePlans()
        {
            //Arrange
            var smartMeterId = "smart-meter-0";
            var limit = 5;
            var maxNumberUsageCost = 3;

            //Action
            var response = await SendAsync(client => client.GetAsync(string.Format($"{Get.Recommend}?limit={limit}", smartMeterId)));

            //Assert
            var result = await RetrieveResultAsync<Dictionary<string, decimal>>(response);
            this.ShouldSatisfyAllConditions(
                () => result.Count.ShouldBe(maxNumberUsageCost),
                () => response.StatusCode.ShouldBe((HttpStatusCode)StatusCodes.Status200OK)
            );
        }

        private static class Get
        {
            public static string CompareAll = "price-plans/compare-all/{0}";
            public static string Recommend = "price-plans/recommend/{0}";
        }
    }
}
