using System;

namespace JOIEnergy.Domain.Entity
{
    public class PeakTimeMultiplier : EntityBase
    {
        public Guid PricePlanId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public decimal Multiplier { get; set; }

        public static PeakTimeMultiplier Create(DayOfWeek dayOfWeek, decimal multiplier)
        {
            return new PeakTimeMultiplier
            {
                DayOfWeek = dayOfWeek,
                Multiplier = multiplier
            };
        }
    }
}