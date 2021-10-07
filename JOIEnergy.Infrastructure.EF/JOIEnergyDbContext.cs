using System.Threading;
using System.Threading.Tasks;
using JOIEnergy.Domain.Entity;
using JOIEnergy.Domain.Interfaces;
using JOIEnergy.Infrastructure.EF.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace JOIEnergy.Infrastructure.EF
{
    public class JOIEnergyDbContext : DbContext, IUnitOfWork
    {
        public JOIEnergyDbContext(DbContextOptions<JOIEnergyDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new AccountConfiguration());
            builder.ApplyConfiguration(new ElectricityReadingConfiguration());
            builder.ApplyConfiguration(new EnergyCompanyConfiguration());
            builder.ApplyConfiguration(new PeakTimeMultiplierConfiguration());
            builder.ApplyConfiguration(new PricePlanConfiguration());
            builder.ApplyConfiguration(new SmartMeterConfiguration());
            base.OnModelCreating(builder);
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<ElectricityReading> ElectricityReadings { get; set; }
        public DbSet<EnergyCompany> EnergyCompanies { get; set; }
        public DbSet<SmartMeter> SmartMeters { get; set; }
        public DbSet<PricePlan> PricePlans { get; set; }
        public DbSet<PeakTimeMultiplier> PeakTimeMultipliers { get; set; }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.SaveChangesAsync(cancellationToken);
        }
    }
}
