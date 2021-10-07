using System;
using System.Collections.Generic;
using JOIEnergy.Application;
using JOIEnergy.Application.Model;
using JOIEnergy.Application.UsageCostPricePlan;
using JOIEnergy.Domain.Enums;
using Moq;
using Xunit;

namespace JOIEnergy.Tests
{
    public class AverageUsageCostPricePlanProviderTest
    {
        private readonly Mock<IMeterReadingService> _mockMeterReadingService;
        private readonly Mock<IPricePlanService> _mockPricePlanService;
        private readonly AverageUsageCostPricePlanProvider _usageCostPricePlanProvider;
        private static string SMART_METER_ID = "smart-meter-id";

        public AverageUsageCostPricePlanProviderTest()
        {
            _mockMeterReadingService = new Mock<IMeterReadingService>();
            _mockPricePlanService = new Mock<IPricePlanService>();

            _mockPricePlanService.Setup(x => x.GetPricePlans())
                .Returns(new List<EnergyCompanyPricePlanModel>()
                {
                    new EnergyCompanyPricePlanModel()
                    {
                        CompanySupplier = Supplier.DrEvilsDarkEnergy,
                        UnitRate = 10,
                        PeakTimeMultipliers = NoMultipliers()
                    },
                    new EnergyCompanyPricePlanModel()
                    {
                        CompanySupplier = Supplier.TheGreenEco,
                        UnitRate = 2,
                        PeakTimeMultipliers = NoMultipliers()
                    },
                    new EnergyCompanyPricePlanModel()
                    {
                        CompanySupplier = Supplier.PowerForEveryone,
                        UnitRate = 1,
                        PeakTimeMultipliers = NoMultipliers()
                    },
                });

            _usageCostPricePlanProvider = new AverageUsageCostPricePlanProvider(_mockPricePlanService.Object, _mockMeterReadingService.Object);
        }

        [Fact]
        public void ShouldCalculateCostForMeterReadingsForEveryPricePlan()
        {
            //Arrange
            _mockMeterReadingService.Setup(x => x.GetReadings(SMART_METER_ID))
                .Returns(new List<ElectricityReadingModel>
                {
                    new ElectricityReadingModel() { Time = DateTime.Now.AddHours(-1), Reading = 15.0m },
                    new ElectricityReadingModel() { Time = DateTime.Now, Reading = 5.0m }
                });

            //Act
            var actualCosts = _usageCostPricePlanProvider.GetConsumptionCostOfElectricityReadingsForEachPricePlan(SMART_METER_ID);

            //Assert
            Assert.Equal(3, actualCosts.Count);
            Assert.Equal(100m, actualCosts["" + Supplier.DrEvilsDarkEnergy], 3);
            Assert.Equal(20m, actualCosts["" + Supplier.TheGreenEco], 3);
            Assert.Equal(10m, actualCosts["" + Supplier.PowerForEveryone], 3);
        }

        [Fact]
        public void ShouldRecommendCheapestPricePlansNoLimitForMeterUsage()
        {
            //Arrange
            _mockMeterReadingService.Setup(x => x.GetReadings(SMART_METER_ID))
                .Returns(new List<ElectricityReadingModel>
                {
                    new ElectricityReadingModel() { Time = DateTime.Now.AddMinutes(-30), Reading = 35m },
                    new ElectricityReadingModel() { Time = DateTime.Now, Reading = 3m }
                });

            //Act
            var actualCosts = _usageCostPricePlanProvider.GetConsumptionCostOfElectricityReadingsForEachPricePlan(SMART_METER_ID);

            //Assert
            Assert.Equal(3, actualCosts.Count);
            Assert.Equal(380m, actualCosts["" + Supplier.DrEvilsDarkEnergy], 3);
            Assert.Equal(76m, actualCosts["" + Supplier.TheGreenEco], 3);
            Assert.Equal(38m, actualCosts["" + Supplier.PowerForEveryone], 3);
        }

        [Fact]
        public void ShouldRecommendLimitedCheapestPricePlansForMeterUsage()
        {
            //Arrange
            _mockMeterReadingService.Setup(x => x.GetReadings(SMART_METER_ID))
                .Returns(new List<ElectricityReadingModel>
                {
                    new ElectricityReadingModel() { Time = DateTime.Now.AddMinutes(-45), Reading = 5m},
                    new ElectricityReadingModel() { Time = DateTime.Now, Reading = 20m }
                });

            //Act
            var actualCosts = _usageCostPricePlanProvider.GetConsumptionCostOfElectricityReadingsForEachPricePlan(SMART_METER_ID);

            //Assert
            Assert.Equal(3, actualCosts.Count);
            Assert.Equal(166.667m, actualCosts["" + Supplier.DrEvilsDarkEnergy], 3);
            Assert.Equal(33.333m, actualCosts["" + Supplier.TheGreenEco], 3);
            Assert.Equal(16.667m, actualCosts["" + Supplier.PowerForEveryone], 3);
        }

        private static List<PeakTimeMultiplierModel> NoMultipliers()
        {
            return new List<PeakTimeMultiplierModel>();
        }
    }
}
