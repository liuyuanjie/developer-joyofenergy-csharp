using System;

namespace JOIEnergy.Domain.Entity
{
    public class SmartMeter: EntityBase
    {
        public string SmartMeterNo { get; set; }

        public ElectricityReading CreateElectricityReading(DateTime time, decimal reading)
        {
            return new ElectricityReading()
            {
                Time = time,
                Reading = reading,
                SmartMeterId = Id
            };
        }
    }
}
