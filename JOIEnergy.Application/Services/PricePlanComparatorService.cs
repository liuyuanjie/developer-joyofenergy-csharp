﻿using System.Collections.Generic;
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
            return GetConsumptionCostOfElectricityReadingsForEachPricePlan(smartMeterId);
        }

        private Dictionary<string, decimal> GetConsumptionCostOfElectricityReadingsForEachPricePlan(string smartMeterId)
        {
            return _usageCostPricePlanProvider.GetConsumptionCostOfElectricityReadingsForEachPricePlan(smartMeterId);
        }

        public Dictionary<string, decimal> GetRecommendedUsageCostPricePlans(string smartMeterId, int? recommendLimit)
        {
            if (!recommendLimit.HasValue || recommendLimit.Value <= 0)
            {
                return GetConsumptionCostOfElectricityReadingsForEachPricePlan(smartMeterId);
            }

            var items = GetConsumptionCostOfElectricityReadingsForEachPricePlan(smartMeterId).OrderBy(x => x.Value);
            var pagingItems = recommendLimit.Value > items.Count()
                ? items
                : items.Take(recommendLimit.Value);

            return pagingItems.ToList().ToDictionary(x => x.Key, x => x.Value);
        }
    }
}