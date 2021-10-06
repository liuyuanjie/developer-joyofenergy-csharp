using JOIEnergy.Domain.Enums;

namespace JOIEnergy.Services
{
    public interface IAccountService
    {
        Supplier GetPricePlanIdForSmartMeterId(string smartMeterId);
    }
}