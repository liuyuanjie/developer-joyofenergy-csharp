using System.Collections.Generic;

namespace JOIEnergy.Domain.Entity
{
    public class MeterReadings
    {
        public string SmartMeterId { get; set; }
        public List<ElectricityReading> ElectricityReadings { get; set; }
    }
}
