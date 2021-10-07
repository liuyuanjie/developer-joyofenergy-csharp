using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JOIEnergy.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JOIEnergy.Infrastructure.EF.EntityConfigurations
{
    public class EnergyCompanyConfiguration : IEntityTypeConfiguration<EnergyCompany>
    {
        public void Configure(EntityTypeBuilder<EnergyCompany> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.HasMany(b => b.PricePlans)
                .WithOne()
                .HasForeignKey(nameof(PricePlan.EnergyCompanyId))
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
