using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JOIEnergy.Application
{
    public interface IPricePlanComparatorService
    {
        Dictionary<string, decimal> GetAllUsageCostPricePlans(string smartMeterId);
        Dictionary<string, decimal> GetRecommendedUsageCostPricePlans(string smartMeterId, int? recommendLimit);
    }
}
