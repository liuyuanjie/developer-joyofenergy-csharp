using System;
using System.Collections.Generic;
using JOIEnergy.Application.Model;
using JOIEnergy.Application.UsageCostPricePlan;
using JOIEnergy.Domain.Enums;
using Xunit;

namespace JOIEnergy.Tests.Application.UsageCostPricePlan
{
    public class AverageUsageCostPricePlanProviderTest: UsageCostPricePlanBase
    {
        private readonly AverageUsageCostPricePlanProvider _usageCostPricePlanProvider;
        private static string SMART_METER_ID = "smart-meter-id";

        public AverageUsageCostPricePlanProviderTest()
        {
            PricePlanServiceMock.Setup(x => x.GetPricePlans())
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

            _usageCostPricePlanProvider = new AverageUsageCostPricePlanProvider(PricePlanServiceMock.Object, MeterReadingServiceMock.Object);
        }

        [Fact]
        public void ShouldCalculateCostForMeterReadingsForEveryPricePlan()
        {
            //Arrange
            MeterReadingServiceMock.Setup(x => x.GetReadings(SMART_METER_ID))
                .Returns(new List<ElectricityReadingModel>
                {
                    new ElectricityReadingModel() { Time = DateTime.Now.AddHours(-1), Reading = 15.0m },
                    new ElectricityReadingModel() { Time = DateTime.Now, Reading = 5.0m }
                });

            //Act
            var actualCosts = _usageCostPricePlanProvider.GetConsumptionCostOfElectricityReadingsForEachPricePlan(SMART_METER_ID);

            //Assert
            Assert.Equal(3, actualCosts.Count);
            Assert.Equal(100m, actualCosts[Supplier.DrEvilsDarkEnergy.ToString()], 3);
            Assert.Equal(20m, actualCosts[Supplier.TheGreenEco.ToString()], 3);
            Assert.Equal(10m, actualCosts[Supplier.PowerForEveryone.ToString()], 3);
        }

        [Fact]
        public void ShouldRecommendCheapestPricePlansNoLimitForMeterUsage()
        {
            //Arrange
            MeterReadingServiceMock.Setup(x => x.GetReadings(SMART_METER_ID))
                .Returns(new List<ElectricityReadingModel>
                {
                    new ElectricityReadingModel() { Time = DateTime.Now.AddMinutes(-30), Reading = 35m },
                    new ElectricityReadingModel() { Time = DateTime.Now, Reading = 3m }
                });

            //Act
            var actualCosts = _usageCostPricePlanProvider.GetConsumptionCostOfElectricityReadingsForEachPricePlan(SMART_METER_ID);

            //Assert
            Assert.Equal(3, actualCosts.Count);
            Assert.Equal(380m, actualCosts[Supplier.DrEvilsDarkEnergy.ToString()], 3);
            Assert.Equal(76m, actualCosts[Supplier.TheGreenEco.ToString()], 3);
            Assert.Equal(38m, actualCosts[Supplier.PowerForEveryone.ToString()], 3);
        }

        [Fact]
        public void ShouldRecommendLimitedCheapestPricePlansForMeterUsage()
        {
            //Arrange
            MeterReadingServiceMock.Setup(x => x.GetReadings(SMART_METER_ID))
                .Returns(new List<ElectricityReadingModel>
                {
                    new ElectricityReadingModel() { Time = DateTime.Now.AddMinutes(-45), Reading = 5m},
                    new ElectricityReadingModel() { Time = DateTime.Now, Reading = 20m }
                });

            //Act
            var actualCosts = _usageCostPricePlanProvider.GetConsumptionCostOfElectricityReadingsForEachPricePlan(SMART_METER_ID);

            //Assert
            Assert.Equal(3, actualCosts.Count);
            Assert.Equal(166.667m, actualCosts[Supplier.DrEvilsDarkEnergy.ToString()], 3);
            Assert.Equal(33.333m, actualCosts[Supplier.TheGreenEco.ToString()], 3);
            Assert.Equal(16.667m, actualCosts[Supplier.PowerForEveryone.ToString()], 3);
        }

        private static List<PeakTimeMultiplierModel> NoMultipliers()
        {
            return new List<PeakTimeMultiplierModel>();
        }
    }
}
