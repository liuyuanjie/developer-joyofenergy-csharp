using System;
using System.Collections.Generic;
using JOIEnergy.Domain.Enums;

namespace JOIEnergy.Application.Model
{
    public class EnergyCompanyPricePlanModel
    {
        public string CompanyName { get; set; }
        public Supplier CompanySupplier { get; set; }
        public Guid PricePlanId { get; set; }
        public string PricePlanName { get; set; }
        public decimal UnitRate { get; set; }

        public IList<PeakTimeMultiplierModel> PeakTimeMultipliers { get; set; }
    }

    public class PeakTimeMultiplierModel
    {
        public DayOfWeek DayOfWeek { get; set; }
        public decimal Multiplier { get; set; }
    }
}
