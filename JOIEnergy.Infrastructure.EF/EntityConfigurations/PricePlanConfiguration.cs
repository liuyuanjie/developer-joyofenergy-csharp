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
    public class PricePlanConfiguration : IEntityTypeConfiguration<PricePlan>
    {
        public void Configure(EntityTypeBuilder<PricePlan> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.UnitRate).HasPrecision(18, 4);
            builder.HasMany(b => b.PeakTimeMultipliers)
                .WithOne()
                .HasForeignKey(nameof(PeakTimeMultiplier.PricePlanId))
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
