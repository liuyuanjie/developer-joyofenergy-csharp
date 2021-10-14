using System.Linq;
using JOIEnergy.Application;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JOIEnergy.Controllers
{
    [ApiController]
    [Route("price-plans")]
    public class PricePlanComparatorController : ControllerBase
    {
        private readonly IPricePlanComparatorService _pricePlanComparatorService;

        public PricePlanComparatorController(IPricePlanComparatorService pricePlanComparatorService)
        {
            _pricePlanComparatorService = pricePlanComparatorService;
        }

        [HttpGet("compare-all/{smartMeterId}")]
        public ObjectResult CalculatedCostForEachPricePlan(string smartMeterId)
        {
            var costPerPricePlan = _pricePlanComparatorService.GetAllUsageCostPricePlans(smartMeterId);
            if (!costPerPricePlan.Any())
            {
                return new NotFoundObjectResult($"Smart Meter ID ({smartMeterId}) not found");
            }

            return
                costPerPricePlan.Any() ?
                new OkObjectResult(costPerPricePlan) :
                new BadRequestObjectResult($"Smart Meter ID ({smartMeterId}) not found");
        }

        [HttpGet("recommend/{smartMeterId}")]
        public ObjectResult RecommendCheapestPricePlans(string smartMeterId, int? limit = null)
        {
            var consumptionForPricePlans = _pricePlanComparatorService.GetRecommendedUsageCostPricePlans(smartMeterId, limit);

            if (!consumptionForPricePlans.Any())
            {
                return new NotFoundObjectResult($"Smart Meter ID ({smartMeterId}) not found");
            }

            return new OkObjectResult(consumptionForPricePlans);
        }
    }
}
