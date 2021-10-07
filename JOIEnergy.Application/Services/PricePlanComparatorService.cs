using System.Collections.Generic;
using System.Linq;

namespace JOIEnergy.Application.Services
{
    public class PricePlanComparatorService : IPricePlanComparatorService
    {
        private readonly IUsageCostPricePlanProvider _usageCostPricePlanProvider;

        public PricePlanComparatorService(IUsageCostPricePlanProvider usageCostPricePlanProvider)
        {
            _usageCostPricePlanProvider = usageCostPricePlanProvider;
        }

        public Dictionary<string, decimal> GetAllUsageCostPricePlans(string smartMeterId)
        {
            return _usageCostPricePlanProvider.GetConsumptionCostOfElectricityReadingsForEachPricePlan(smartMeterId);
        }

        public Dictionary<string, decimal> GetRecommendedUsageCostPricePlans(string smartMeterId, int recommendLimit)
        {
            var items = _usageCostPricePlanProvider.GetConsumptionCostOfElectricityReadingsForEachPricePlan(smartMeterId)
                .OrderBy(x => x.Value);
            return ((recommendLimit > items.Count() ? items : items.Take(recommendLimit)).ToList()).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}