namespace JOIEnergy.Domain.Entity
{
    public class EnergyCompany: EntityBase
    {
        public string Name { get; set; }
        public PricePlan PricePlan { get; set; }
    }
}
