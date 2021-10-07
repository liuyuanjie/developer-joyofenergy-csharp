using System;
using System.Collections.Generic;
using System.Linq;

namespace JOIEnergy.Domain.Entity
{
    public class PricePlan : EntityBase
    {
        public Guid EnergyCompanyId { get; set; }
        public string Name { get; set; }
        public decimal UnitRate { get; set; }
        public virtual IList<PeakTimeMultiplier> PeakTimeMultipliers { get; set;}

        public decimal GetPrice(DateTime datetime) {
            var multiplier = PeakTimeMultipliers.FirstOrDefault(m => m.DayOfWeek == datetime.DayOfWeek);

            if (multiplier?.Multiplier != null) {
                return multiplier.Multiplier * UnitRate;
            } else {
                return UnitRate;
            }
        }

        public static PricePlan Create(string name, decimal unitRate)
        {
            return new PricePlan
            {
                Name = name,
                UnitRate = unitRate
            };
        }

        public void AddPeakTimeMultiplier(DayOfWeek dayOfWeek, decimal multiplier)
        {
            if (PeakTimeMultipliers == null)
            {
                PeakTimeMultipliers = new List<PeakTimeMultiplier>();
            }

            PeakTimeMultipliers.Add(PeakTimeMultiplier.Create(dayOfWeek, multiplier));
        }
    }
}
