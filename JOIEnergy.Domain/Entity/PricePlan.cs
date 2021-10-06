using System;
using System.Collections.Generic;
using System.Linq;
using JOIEnergy.Domain.Enums;

namespace JOIEnergy.Domain.Entity
{
    public class PricePlan : EntityBase
    {
        public Supplier EnergySupplier { get; set; }
        public decimal UnitRate { get; set; }
        public IList<PeakTimeMultiplier> PeakTimeMultiplier { get; set;}

        public decimal GetPrice(DateTime datetime) {
            var multiplier = PeakTimeMultiplier.FirstOrDefault(m => m.DayOfWeek == datetime.DayOfWeek);

            if (multiplier?.Multiplier != null) {
                return multiplier.Multiplier * UnitRate;
            } else {
                return UnitRate;
            }
        }
    }

    public class PeakTimeMultiplier
    {
        public Guid PricePlanId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public decimal Multiplier { get; set; }
    }
}
