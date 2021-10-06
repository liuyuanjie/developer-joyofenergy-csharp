using System;

namespace JOIEnergy.Domain.Entity
{
    public class ElectricityReading: EntityBase
    {
        public Guid SmartMeterId { get; set; }
        public DateTime Time { get; set; }
        public Decimal Reading { get; set; }
    }
}
