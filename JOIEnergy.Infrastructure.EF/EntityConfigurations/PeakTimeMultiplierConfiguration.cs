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
    public class PeakTimeMultiplierConfiguration : IEntityTypeConfiguration<PeakTimeMultiplier>
    {
        public void Configure(EntityTypeBuilder<PeakTimeMultiplier> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.Multiplier).HasPrecision(18, 4);
        }
    }
}
