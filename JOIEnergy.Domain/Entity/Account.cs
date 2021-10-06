using System;

namespace JOIEnergy.Domain.Entity
{
    public class Account: EntityBase
    {
        public string Name { get; set; }
        public Guid SmartMeterId { get; set; }
        public Guid EnergyCompanyId { get; set; }
    }
}
