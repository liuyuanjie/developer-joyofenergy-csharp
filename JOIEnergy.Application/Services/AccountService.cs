using Dapper;
using JOIEnergy.Application.Model;

namespace JOIEnergy.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IConnection _connection;

        public AccountService(IConnection connection)
        {
            _connection = connection;
        }

        public AccountEnergyCompanyPricePlanModel GetAccountEnergyCompanyPricePlanForSmartMeterId(string smartMeterId)
        {
            var sql = @"SELECT e.[Name] AS [CompanyName], e.[Supplier] AS [CompanySupplier], p.[Name] AS [PricePlanName], a.[Name] AS [AccountName] FROM [EnergyCompanies] AS e 
                      INNER JOIN [PricePlans] AS p ON e.[Id] = p.[EnergyCompanyId]
                      INNER JOIN [SmartMeters] AS s ON p.[Id] = s.[EnergyCompanyPricePlanId]
                      INNER JOIN [Accounts] AS a ON a.[Id] = s.[AccountId]
                      WHERE s.[SmartMeterId] = @smartMeterId";
            using (var conn = _connection.OpenConnection())
            {
                var result = conn.ExecuteScalar<AccountEnergyCompanyPricePlanModel>(sql, new { smartMeterId });
                return result ?? new AccountEnergyCompanyPricePlanModel();
            }
        }
    }
}
