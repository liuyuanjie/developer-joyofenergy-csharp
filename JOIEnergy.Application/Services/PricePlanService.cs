using System.Collections.Generic;
using System.Linq;
using Dapper;
using JOIEnergy.Application.Model;

namespace JOIEnergy.Application.Services
{
    public class PricePlanService : IPricePlanService
    {
        private readonly IConnection _connection;

        public PricePlanService(IConnection connection)
        {
            _connection = connection;
        }

        public List<EnergyCompanyPricePlanModel> GetPricePlans()
        {
            List<EnergyCompanyPricePlanModel> result;
            var sql = "SELECT e.[Name] AS [CompanyName], e.[Supplier] AS [CompanySupplier], p.[Id] AS [PricePlanId], p.[Name] AS [PricePlanName], p.[UnitRate] FROM [PricePlans] AS p " +
                      "INNER JOIN [EnergyCompanies] AS e ON e.[Id] = p.[EnergyCompanyId]";
            using (var conn = _connection.OpenConnection())
            {
                result = conn.Query<EnergyCompanyPricePlanModel>(sql).ToList();
            }

            result.ForEach(p => { GetPeakTimeMultipliers(p); });

            return result;
        }

        private void GetPeakTimeMultipliers(EnergyCompanyPricePlanModel model)
        {
            var sql = "SELECT p.[DayOfWeek], p.[Multiplier] FROM [PeakTimeMultipliers] AS p " +
                      "WHERE p.[PricePlanId] = @pricePlanId";
            using (var conn = _connection.OpenConnection())
            {
                model.PeakTimeMultipliers = conn.Query<PeakTimeMultiplierModel>(sql, new { pricePlanId = model.PricePlanId }).ToList();
            }
        }
    }
}
