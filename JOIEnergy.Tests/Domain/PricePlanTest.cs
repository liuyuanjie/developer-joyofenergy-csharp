using System;
using System.Collections.Generic;
using JOIEnergy.Domain.Entity;
using JOIEnergy.Domain.Enums;
using Xunit;

namespace JOIEnergy.Tests.Domain
{
    public class PricePlanTest
    {
        private readonly PricePlan _pricePlan;

        public PricePlanTest()
        {
            _pricePlan = new PricePlan
            {
                Name = Supplier.TheGreenEco.ToString(),
                UnitRate = 20m,
                PeakTimeMultipliers = new List<PeakTimeMultiplier> {
                    new PeakTimeMultiplier { 
                        DayOfWeek = DayOfWeek.Saturday,
                        Multiplier = 2m
                    },
                    new PeakTimeMultiplier {
                        DayOfWeek = DayOfWeek.Sunday,
                        Multiplier = 10m
                    }
                }
            };
        }

        [Fact]
        public void TestGetEnergySupplier() {
            Assert.Equal(Supplier.TheGreenEco.ToString(), _pricePlan.Name);
        }

        [Fact]
        public void TestGetBasePrice() {
            Assert.Equal(20m, _pricePlan.GetPrice(new DateTime(2018, 1, 2)));
        }

        [Fact]
        public void TestGetPeakTimePrice()
        {
            Assert.Equal(40m, _pricePlan.GetPrice(new DateTime(2018, 1, 6)));
        }

    }
}
