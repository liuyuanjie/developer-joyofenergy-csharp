using System.Collections.Generic;
using System.Linq;
using JOIEnergy.Application;
using JOIEnergy.Application.Services;
using JOIEnergy.Application.UsageCostPricePlan;
using JOIEnergy.Domain.Enums;
using Moq;
using Xunit;

namespace JOIEnergy.Tests.Application.Services
{
    public class PricePlanComparatorServiceTest : ServiceBase
    {
        private static string SMART_METER_ID = "smart-meter-id";
        private readonly PricePlanComparatorService _pricePlanComparatorService;
        private readonly Mock<IUsageCostPricePlanProvider> _averageUsageCostPricePlanProviderMock;

        public PricePlanComparatorServiceTest()
        {
            _averageUsageCostPricePlanProviderMock = new Mock<IUsageCostPricePlanProvider>();
            _averageUsageCostPricePlanProviderMock.Setup(x =>
                    x.GetConsumptionCostOfElectricityReadingsForEachPricePlan(SMART_METER_ID))
                .Returns(new Dictionary<string, decimal>(new List<KeyValuePair<string, decimal>>()
                {
                    new KeyValuePair<string, decimal>(Supplier.TheGreenEco.ToString(), 21.806m),
                    new KeyValuePair<string, decimal>(Supplier.PowerForEveryone.ToString(), 10.903m),
                    new KeyValuePair<string, decimal>(Supplier.DrEvilsDarkEnergy.ToString(), 100.903m),
                }));
            _pricePlanComparatorService = new PricePlanComparatorService(_averageUsageCostPricePlanProviderMock.Object);
        }

        [Fact]
        public void GivenRecommendLimitIsNullShouldReturnAllRecommendedUsageCostPricePlans()
        {
            //Arrange

            //Act
            var result = _pricePlanComparatorService.GetRecommendedUsageCostPricePlans(SMART_METER_ID, null);

            //Assert
            Assert.Equal(3, result.Count);
            Assert.Equal(true, result.First().Value < result.Skip(1).First().Value);
            Assert.Equal(true, result.Skip(1).First().Value < result.Last().Value);
        }

        [Fact]
        public void GivenRecommendLimitIsLessThanPricePansShouldReturnRecommendedLimitUsageCostPricePlans()
        {
            //Arrange
            var recommendLimit = 2;

            //Act
            var result = _pricePlanComparatorService.GetRecommendedUsageCostPricePlans(SMART_METER_ID, recommendLimit);

            //Assert
            Assert.Equal(recommendLimit, result.Count);
            Assert.Equal(true, result.First().Value < result.Last().Value);
        }

        [Fact]
        public void GivenRecommendLimitMoreThanPricePansShouldReturnRecommendedAllUsageCostPricePlans()
        {
            //Arrange
            var recommendLimit = 5;

            //Act
            var result = _pricePlanComparatorService.GetRecommendedUsageCostPricePlans(SMART_METER_ID, recommendLimit);

            //Assert
            Assert.Equal(3, result.Count);
            Assert.Equal(true, result.First().Value < result.Skip(1).First().Value);
            Assert.Equal(true, result.Skip(1).First().Value < result.Last().Value);
        }
    }
}
