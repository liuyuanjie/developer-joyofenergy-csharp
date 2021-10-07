using JOIEnergy.Application.Model;

namespace JOIEnergy.Application
{
    public interface IAccountService
    {
        AccountEnergyCompanyPricePlanModel GetAccountEnergyCompanyPricePlanForSmartMeterId(string smartMeterId);
    }
}