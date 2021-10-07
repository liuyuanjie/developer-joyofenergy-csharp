using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace JOIEnergy.Domain.Entity
{
    public class SmartMeter: EntityBase
    {
        public Guid? AccountId { get; set; }
        public string SmartMeterId { get; set; }
        public Guid EnergyCompanyPricePlanId { get; set; }

        public virtual List<ElectricityReading> ElectricityReadings { get; set; }

        public static SmartMeter Create(string smartMeterId)
        {
            return new SmartMeter
            {
                SmartMeterId = smartMeterId
            };
        }

        public SmartMeter SetAccount(Guid accountId)
        {
            this.AccountId = accountId;
            return this;
        }

        public SmartMeter SetEnergyCompanyPricePlanId(Guid energyCompanyPricePlanId)
        {
            this.EnergyCompanyPricePlanId = energyCompanyPricePlanId;
            return this;
        }

        public void CreateElectricityReading(DateTime time, decimal reading)
        {
            if (ElectricityReadings == null)
            {
                ElectricityReadings = new List<ElectricityReading>();
            }

            ElectricityReadings.Add(new ElectricityReading()
            {
                Time = time,
                Reading = reading
            });
        }
    }
}
