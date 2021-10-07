using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JOIEnergy.Domain.Entity;
using JOIEnergy.Domain.Enums;
using JOIEnergy.Domain.Interfaces;

namespace JOIEnergy.Infrastructure.EF
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly JOIEnergyDbContext _dbContext;

        public DatabaseInitializer(JOIEnergyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Seed()
        {
            if (_dbContext.Set<Account>().Any())
            {
                return;
            }

            var energyCompanies = CreateEnergyCompanies();
            var accounts = CreateAccounts();
            var pricePlans = CreatePricePlans(energyCompanies);
            var smartMeters = CreateSmartMeters(accounts, pricePlans, energyCompanies);

            await _dbContext.Accounts.AddRangeAsync(accounts);
            await _dbContext.EnergyCompanies.AddRangeAsync(energyCompanies);
            await _dbContext.PricePlans.AddRangeAsync(pricePlans);
            await _dbContext.SmartMeters.AddRangeAsync(smartMeters);

            await _dbContext.SaveChangesAsync();
        }

        private List<EnergyCompany> CreateEnergyCompanies()
        {
            var energyCompanies = new List<EnergyCompany>()
            {
                EnergyCompany.Create("Dr Evil's Dark Energy",Supplier.DrEvilsDarkEnergy),
                EnergyCompany.Create("The Green Eco",Supplier.TheGreenEco),
                EnergyCompany.Create("Power for Everyone",Supplier.PowerForEveryone)
            };

            return energyCompanies;
        }

        private List<PricePlan> CreatePricePlans(IList<EnergyCompany> energyCompanies)
        {
            var pricePlans = new List<PricePlan>()
            {
                new PricePlan
                {
                    EnergyCompanyId = energyCompanies.First(x => x.Supplier == Supplier.DrEvilsDarkEnergy).Id,
                    Name = energyCompanies.First(x => x.Supplier == Supplier.DrEvilsDarkEnergy).Supplier.ToString(),
                    UnitRate = 10m,
                },
                new PricePlan
                {
                    EnergyCompanyId = energyCompanies.First(x => x.Supplier == Supplier.TheGreenEco).Id,
                    Name = energyCompanies.First(x => x.Supplier == Supplier.TheGreenEco).Supplier.ToString(),
                    UnitRate = 2m,
                },
                new PricePlan
                {
                    EnergyCompanyId = energyCompanies.First(x => x.Supplier == Supplier.PowerForEveryone).Id,
                    Name = energyCompanies.First(x => x.Supplier == Supplier.PowerForEveryone).Supplier.ToString(),
                    UnitRate = 1m,
                }
            };

            return pricePlans;
        }

        private List<Account> CreateAccounts()
        {
            var accounts = new List<Account>()
            {
                Account.Create("Sarah"),
                Account.Create("Peter"),
                Account.Create("Charlie"),
                Account.Create("Andrea"),
                Account.Create("Alex")
            };

            return accounts;
        }

        private List<SmartMeter> CreateSmartMeters(IList<Account> accounts, IList<PricePlan> pricePlans, IList<EnergyCompany> energyCompanies)
        {
            var smartMeters = new List<SmartMeter>()
            {
                SmartMeter.Create("smart-meter-0")
                          .SetAccount(accounts.First(x => x.Name == "Sarah").Id)
                          .SetEnergyCompanyPricePlanId(pricePlans.First(x => x.EnergyCompanyId == energyCompanies.First(e => e.Name == "Dr Evil's Dark Energy").Id).Id),
                SmartMeter.Create("smart-meter-1")
                    .SetAccount(accounts.First(x => x.Name == "Peter").Id)
                    .SetEnergyCompanyPricePlanId(pricePlans.First(x => x.EnergyCompanyId == energyCompanies.First(e => e.Name == "The Green Eco").Id).Id),
                SmartMeter.Create("smart-meter-2")
                    .SetAccount(accounts.First(x => x.Name == "Charlie").Id)
                    .SetEnergyCompanyPricePlanId(pricePlans.First(x => x.EnergyCompanyId == energyCompanies.First(e => e.Name == "Dr Evil's Dark Energy").Id).Id),
                SmartMeter.Create("smart-meter-3")
                    .SetAccount(accounts.First(x => x.Name == "Andrea").Id)
                    .SetEnergyCompanyPricePlanId(pricePlans.First(x => x.EnergyCompanyId == energyCompanies.First(e => e.Name == "Power for Everyone").Id).Id),
                SmartMeter.Create("smart-meter-4")
                    .SetAccount(accounts.First(x => x.Name == "Alex").Id)
                    .SetEnergyCompanyPricePlanId(pricePlans.First(x => x.EnergyCompanyId == energyCompanies.First(e => e.Name == "The Green Eco").Id).Id),
            };

            smartMeters.ForEach(x => GenerateMeterReadings(x, 20));

            return smartMeters;
        }

        private void GenerateMeterReadings(SmartMeter smartMeter, int number)
        {
            var readings = new List<ElectricityReading>();
            var random = new Random();
            for (var i = number; i > 0; i--)
            {
                var reading = (decimal)random.NextDouble();
                var time = DateTime.Now.AddSeconds(-i * 10);
                smartMeter.CreateElectricityReading(time, reading);
            }
        }
    }
}