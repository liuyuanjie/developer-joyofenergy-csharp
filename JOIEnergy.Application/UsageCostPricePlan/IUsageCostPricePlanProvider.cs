using System.Collections.Generic;

namespace JOIEnergy.Application.UsageCostPricePlan
{
    public interface IUsageCostPricePlanProvider
    {
        Dictionary<string, decimal> GetConsumptionCostOfElectricityReadingsForEachPricePlan(string smartMeterId);
    }
}
