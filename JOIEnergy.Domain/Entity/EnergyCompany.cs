using System;
using System.Collections.Generic;
using System.Linq;
using JOIEnergy.Domain.Enums;

namespace JOIEnergy.Domain.Entity
{
    public class EnergyCompany : EntityBase
    {
        public string Name { get; set; }
        public Supplier Supplier { get; set; }

        public virtual IList<PricePlan> PricePlans { get; set; }

        public static EnergyCompany Create(string name, Supplier supplier)
        {
            return new EnergyCompany
            {
                Name = name,
                Supplier = supplier
            };
        }

        public void AddPricePlan(string name, decimal unitRate)
        {
            if (PricePlans == null)
            {
                PricePlans = new List<PricePlan>();
            }

            PricePlans.Add(PricePlan.Create(name, unitRate));
        }

        public void AddPricePlanPeakTimeMultiplier(Guid pricePlanId, DayOfWeek dayOfWeek, decimal multiplier)
        {
            var pricePlan = PricePlans.FirstOrDefault(p => p.Id == pricePlanId);
            if (pricePlan == null)
            {
                throw new DomainException("Not found the price plan.");
            }

            pricePlan.PeakTimeMultipliers.Add(PeakTimeMultiplier.Create(dayOfWeek, multiplier));
        }
    }
}
