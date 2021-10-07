using System.Collections.Generic;
using System.Linq;
using JOIEnergy.Application.Model;

namespace JOIEnergy.Application.UsageCostPricePlan
{
    public class AverageUsageCostPricePlanProvider : IUsageCostPricePlanProvider
    {
        private readonly IPricePlanService _pricePlanService;
        private readonly IMeterReadingService _meterReadingService;

        public AverageUsageCostPricePlanProvider(IPricePlanService pricePlanService, IMeterReadingService meterReadingService)
        {
            _pricePlanService = pricePlanService;
            _meterReadingService = meterReadingService;
        }

        public Dictionary<string, decimal> GetConsumptionCostOfElectricityReadingsForEachPricePlan(string smartMeterId)
        {
            var electricityReadings = _meterReadingService.GetReadings(smartMeterId);

            if (!electricityReadings.Any())
            {
                return new Dictionary<string, decimal>();
            }

            var pricePlans = _pricePlanService.GetPricePlans();
            return pricePlans.ToDictionary(plan => plan.CompanySupplier.ToString(), plan => CalculateCost(electricityReadings, plan));
        }

        private decimal CalculateAverageReading(List<ElectricityReadingModel> electricityReadings)
        {
            var newSummedReadings = electricityReadings.Select(readings => readings.Reading).Aggregate((reading, accumulator) => reading + accumulator);

            return newSummedReadings / electricityReadings.Count();
        }

        private decimal CalculateTimeElapsed(List<ElectricityReadingModel> electricityReadings)
        {
            var first = electricityReadings.Min(reading => reading.Time);
            var last = electricityReadings.Max(reading => reading.Time);

            return (decimal)(last - first).TotalHours;
        }
        private decimal CalculateCost(List<ElectricityReadingModel> electricityReadings, EnergyCompanyPricePlanModel pricePlan)
        {
            var average = CalculateAverageReading(electricityReadings);
            var timeElapsed = CalculateTimeElapsed(electricityReadings);
            var averagedCost = average / timeElapsed;
            return averagedCost * pricePlan.UnitRate;
        }
    }
}
