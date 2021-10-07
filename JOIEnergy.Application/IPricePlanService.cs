using System.Collections.Generic;
using JOIEnergy.Application.Model;

namespace JOIEnergy.Application
{
    public interface IPricePlanService
    {
        List<EnergyCompanyPricePlanModel> GetPricePlans();
    }
}