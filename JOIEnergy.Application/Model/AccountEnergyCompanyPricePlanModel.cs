using JOIEnergy.Domain.Enums;

namespace JOIEnergy.Application.Model
{
    public class AccountEnergyCompanyPricePlanModel
    {
        public string CompanyName { get; set; }
        public Supplier CompanySupplier { get; set; } = Supplier.NullSupplier;
        public string PricePlanName { get; set; }
        public string AccountName { get; set; }
    }
}
